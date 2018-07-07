using System;
using System.Data.SQLite;
using System.IO;
using System.Data;
using NLog;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace NVidia_Surround_Assistant
{
    public class SQL
    {
        //SQL
        SQLiteConnection m_dbConnection = null;
        SQLiteCommand command;

        #region Public: Connection Management
        public bool SQL_OpenConnection(string SQLiteDbName)
        {
            try
            {
                if (m_dbConnection != null && m_dbConnection.State != ConnectionState.Closed)
                    m_dbConnection.Close();

                if (!File.Exists(SQLiteDbName))
                {
                    NVidia_Surround_Assistant.Properties.Settings.Default.SQLiteDbName = SQLiteDbName;
                    //Create db and all relevant tables
                    SQLiteConnection.CreateFile(SQLiteDbName);
                    m_dbConnection = new SQLiteConnection($"Data Source={SQLiteDbName};Version=3;");
                    m_dbConnection.Open();                    

                    SQL_ExecuteNonQuery("CREATE TABLE ApplicationList ( id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, enabled BOOLEAN DEFAULT 'true'," +
                        "DisplayName STRING(256), fullPath STRING(260) UNIQUE, image BLOB(20971520), surroundGrid INTEGER DEFAULT 0," +
                        "pauseOnDetect BOOLEAN DEFAULT 'true', switchbackTimeout INTEGER DEFAULT 5");//20mb file
                    SQL_ExecuteNonQuery("CREATE TABLE SurroundConfigs ( id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT UNIQUE, ConfigFile BLOB )");
                }
                else
                {
                    m_dbConnection = new SQLiteConnection($"Data Source=\"{SQLiteDbName}\";Version=3;");
                    m_dbConnection.Open();
                }
            }
            catch (SQLiteException ex)
            {
                MainForm.logger.Debug("SQLite: SQL Open: {0}", ex.Message);
            }
            catch (System.DllNotFoundException ex)
            {
                MessageBox.Show("Could not open SQL database.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Fatal("DLL not Found: SQL Open: {0}", ex.Message);
            }
            catch (BadImageFormatException ex)
            {
                MainForm.logger.Fatal("Wrong DLL Architecture: SQL Open: {0}", ex.Message);
            }

            if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                return true;
            else
                return false;
        }

        public bool SQL_CloseConnection()
        {
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    m_dbConnection.Close();
                }
            }
            catch (SQLiteException ex)
            {
                MainForm.logger.Debug("SQL: Close: {0}", ex.Message);
            }

            if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Closed)
                return true;
            else
                return false;
        }
        #endregion

        #region Public: ApplicationInfo Table
        public List<ApplicationInfo> GetApplicationList()
        {
            List<ApplicationInfo> applicationInfos = new List<ApplicationInfo>();

            SQLiteDataReader reader = SQL_ExecuteQuery("SELECT * FROM ApplicationList");
            if (reader != null)
            {
                if (reader.VisibleFieldCount > 0)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            applicationInfos.Add(new ApplicationInfo
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Enabled = (bool)reader["enabled"],
                                DisplayName = (string)reader["DisplayName"],
                                FullPath = (string)reader["fullPath"],
                                ProcessName = Path.GetFileNameWithoutExtension((string)reader["fullPath"]),
                                Image = new Bitmap(ByteToImage((byte[])reader["image"])),
                                SurroundGrid = reader.GetInt32(reader.GetOrdinal("surroundGrid")),
                                PauseOnDetect = (bool)reader["pauseOnDetect"],
                                SwitchbackTimeout = reader.GetInt32(reader.GetOrdinal("switchbackTimeout")),
                            });
                        }
                        catch (System.InvalidCastException ex)
                        {
                            MainForm.logger.Debug("Invalid Cast: {0}", ex.Message);
                        }
                    }
                }
            }
            return applicationInfos;
        }

        public int AddApplication(ApplicationInfo newApp)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@enabled", newApp.Enabled), new SQLiteParameter("@DisplayName", newApp.DisplayName), new SQLiteParameter("@fullPath", newApp.FullPath),
                new SQLiteParameter("@image", ImageToByte(newApp.Image)), new SQLiteParameter("@surroundGrid", newApp.SurroundGrid),
                new SQLiteParameter("@pauseOnDetect", newApp.PauseOnDetect), new SQLiteParameter("@switchbackTimeout", newApp.SwitchbackTimeout) };
            if (SQL_ExecuteNonQuery("INSERT INTO ApplicationList (enabled,  DisplayName, fullPath, image, surroundGrid, pauseOnDetect, switchbackTimeout) values (@enabled, @DisplayName, @fullPath, @image, @surroundGrid, @pauseOnDetect, @switchbackTimeout)", parameters) > 0)
            {
                SQLiteDataReader reader = SQL_ExecuteQuery("SELECT * FROM ApplicationList WHERE DisplayName = @DisplayName", parameters);
                if (reader != null)
                {
                    if (reader.VisibleFieldCount > 0)
                    {
                        reader.Read();
                        return reader.GetInt32(reader.GetOrdinal("id"));
                    }
                }
            }
            return -1;
        }

        public bool UpdateApplication(ApplicationInfo editApp)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@id", editApp.Id), new SQLiteParameter("@enabled", editApp.Enabled), new SQLiteParameter("@DisplayName", editApp.DisplayName), new SQLiteParameter("@fullPath", editApp.FullPath),
                new SQLiteParameter("@image", ImageToByte(editApp.Image)), new SQLiteParameter("@surroundGrid", editApp.SurroundGrid),
                new SQLiteParameter("@pauseOnDetect", editApp.PauseOnDetect), new SQLiteParameter("@switchbackTimeout", editApp.SwitchbackTimeout) };
        
            if (SQL_ExecuteNonQuery("UPDATE ApplicationList SET enabled = @Enabled, DisplayName = @DisplayName, fullPath = @fullPath, image = @image, surroundGrid = @surroundGrid, pauseOnDetect = @pauseOnDetect, switchbackTimeout = @switchbackTimeout WHERE id = @id", parameters) > 0)
                return true;
            else
                return false;
        }

        public bool DeleteApplication(int appId)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@id", appId) };

            if (SQL_ExecuteNonQuery("DELETE FROM ApplicationList WHERE id = @id", parameters) > 0)
            {
                return true;
            }
            return false;
        }

        public bool EnableApplication(int appId)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@Enabled", true), new SQLiteParameter("@id", appId) };

            if (SQL_ExecuteNonQuery("UPDATE ApplicationList SET enabled = @Enabled WHERE id = @id", parameters) > 0)
            {
                return true;
            }
            return false;
        }

        public bool DisableApplication(int appId)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@Enabled", false), new SQLiteParameter("@id", appId) };

            if (SQL_ExecuteNonQuery("UPDATE ApplicationList SET enabled = @Enabled WHERE id = @id", parameters) > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Public: SurroundConfig Table
        public List<SurroundConfig> GetSurroundConfigList()
        {
            List<SurroundConfig> surroundConfigs = new List<SurroundConfig>();

            SQLiteDataReader reader = SQL_ExecuteQuery("SELECT * FROM SurroundConfigs");
            if (reader != null)
            {
                if (reader.VisibleFieldCount > 0)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            surroundConfigs.Add(new SurroundConfig
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = (string)reader["Name"],
                                Config = (byte[])reader["ConfigFile"],
                            });
                        }
                        catch (System.InvalidCastException ex)
                        {
                            MainForm.logger.Debug("Invalid Cast: {0}", ex.Message);
                        }
                    }
                }
            }
            return surroundConfigs;
        }

        public bool SetSurroundConfig(SurroundConfig newConfig)
        {
            bool result = true;

            if (!SurroundConfigExists(newConfig.Name))
            {
                AddSurroundConfig(newConfig);
            }
            else
            {
                UpdateSurroundConfig(newConfig);
            }
            return result;
        }

        public SurroundConfig GetSurroundConfig(string configName)
        {
            SurroundConfig resultConfig = null;

            if(SurroundConfigExists(configName))
            {
                SQLiteDataReader reader = SQL_ExecuteQuery(String.Format("SELECT * FROM SurroundConfigs WHERE Name = \"{0}\"", configName));
                if (reader != null)
                {
                    resultConfig = ReadSurroundConfig(reader);
                }
            }

            return resultConfig;
        }

        public SurroundConfig GetSurroundConfig(int configId)
        {
            SurroundConfig resultConfig = null;

            if (SurroundConfigExists(configId))
            {
                SQLiteDataReader reader = SQL_ExecuteQuery(String.Format("SELECT * FROM SurroundConfigs WHERE id = \"{0}\"", configId));
                if (reader != null)
                {
                    resultConfig = ReadSurroundConfig(reader);
                }
            }

            return resultConfig;
        }

        public bool DeleteSurroundConfig(SurroundConfig config)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@id", config.Id) };

            if (SQL_ExecuteNonQuery("DELETE FROM SurroundConfigs WHERE id = @id", parameters) > 0)
            {
                return true;
            }
            return false;
        }

        public bool DeleteSurroundConfig(string configName)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@name", configName) };

            if (SQL_ExecuteNonQuery("DELETE FROM SurroundConfigs WHERE Name = @name", parameters) > 0)
            {
                return true;
            }
            return false;
        }

        public bool DeleteSurroundConfig(int configId)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@id", configId) };

            if (SQL_ExecuteNonQuery("DELETE FROM SurroundConfigs WHERE id = @id", parameters) > 0)
            {
                return true;
            }
            return false;
        }

        public bool SurroundConfigExists(string configName)
        {
            bool result = false;

            SQLiteDataReader reader = SQL_ExecuteQuery(String.Format("SELECT * FROM SurroundConfigs WHERE Name = \"{0}\"", configName));
            if (reader != null)
            {
                if (ReadSurroundConfig(reader) != null)
                    result = true;
            }
            return result;
        }

        public bool SurroundConfigExists(int configId)
        {
            bool result = false;

            SQLiteDataReader reader = SQL_ExecuteQuery(String.Format("SELECT * FROM SurroundConfigs WHERE id = \"{0}\"", configId));
            if (reader != null)
            {
                if (ReadSurroundConfig(reader) != null)
                    result = true;
            }
            return result;
        }

        private int AddSurroundConfig(SurroundConfig newConfig)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@name", newConfig.Name), new SQLiteParameter("@config", newConfig.Config) };

            if (SQL_ExecuteNonQuery("INSERT INTO SurroundConfigs (Name, ConfigFile) values (@name, @config)", parameters) > 0)
            {
                SQLiteDataReader reader = SQL_ExecuteQuery("SELECT * FROM SurroundConfigs WHERE Name = \"@name\"", parameters);
                if (reader != null)
                {
                    if (reader.VisibleFieldCount > 0)
                    {
                        reader.Read();
                        return reader.GetInt32(reader.GetOrdinal("id"));
                    }
                }
            }

            return -1;
        }

        private bool UpdateSurroundConfig(SurroundConfig editConfig)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@id", editConfig.Id), new SQLiteParameter("@name", editConfig.Name), new SQLiteParameter("@config", editConfig.Config) };

            if (SQL_ExecuteNonQuery("UPDATE SurroundConfigs SET Name = @name, ConfigFile = @config WHERE id = @id", parameters) > 0)
                return true;
            else
                return false;

        }      

        private SurroundConfig ReadSurroundConfig(SQLiteDataReader reader)
        {
            SurroundConfig surroundConfig = null;
            if (reader.VisibleFieldCount > 0 && reader.StepCount > 0)
            {
                while (reader.Read())
                {
                    try
                    {
                        surroundConfig = new SurroundConfig
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = (string)reader["Name"],
                            Config = (byte[])reader["ConfigFile"],
                        };
                    }
                    catch (System.InvalidCastException ex)
                    {
                        MainForm.logger.Debug("Invalid Cast: {0}", ex.Message);
                    }
                }
            }
            return surroundConfig;
        }
        #endregion

        #region Private: Query Functions
        private int SQL_ExecuteNonQuery(string sqlCommand)
        {
            int result = 0;
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    result = command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                MainForm.logger.Debug("SQL: Execute No Query: {0}", ex.Message);
                result = -1;
            }
            return result;
        }

        private int SQL_ExecuteNonQuery(string sqlCommand, SQLiteParameter[] parameters)
        {
            int result = 0;
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    command.Parameters.AddRange(parameters);
                    result = command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                MainForm.logger.Debug("SQL: Execute No Query: {0}", ex.Message);
                result = -1;
            }
            return result;
        }

        private SQLiteDataReader SQL_ExecuteQuery(string sqlCommand)
        {
            SQLiteDataReader reader = null;
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    reader = command.ExecuteReader();
                }
            }
            catch (SQLiteException ex)
            {
                MainForm.logger.Debug("SQL: Execute: {0}", ex.Message);
            }
            return reader;
        }

        private SQLiteDataReader SQL_ExecuteQuery(string sqlCommand, SQLiteParameter[] parameters)
        {
            SQLiteDataReader reader = null;
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    command.Parameters.AddRange(parameters);
                    reader = command.ExecuteReader();
                }
            }
            catch (SQLiteException ex)
            {
                MainForm.logger.Debug("SQL: Execute: {0}", ex.Message);
            }
            return reader;
        }       

        private static byte[] ImageToByte(Image img)
        {
            if (img != null)
            {
                using (var stream = new MemoryStream())
                {
                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return stream.ToArray();
                }
            }
            return null;
        }

        private static Image ByteToImage(byte[] byteImg)
        {
            if (byteImg != null)
            {
                using (var stream = new MemoryStream(byteImg))
                {
                    return Image.FromStream(stream);
                }
            }
            return null;
        }
        #endregion
    }
}
