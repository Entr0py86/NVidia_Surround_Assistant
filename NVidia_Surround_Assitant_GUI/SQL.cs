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
        Logger logger = LogManager.GetLogger("nvsaLogger");
        //Sql
        SQLiteConnection m_dbConnection = null;
        SQLiteCommand command;

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
                    SQL_ExecuteNonQuery("CREATE TABLE ApplicationList (id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, enabled BOOLEAN, DisplayName STRING (256), fullPath STRING (260) UNIQUE, image BLOB (20971520))");//20mb file
                }
                else
                {
                    m_dbConnection = new SQLiteConnection($"Data Source=\"{SQLiteDbName}\";Version=3;");
                    m_dbConnection.Open();
                }
            }
            catch (SQLiteException ex)
            {
                logger.Debug("SQLite: SQL Open: {0}", ex.Message);
            }
            catch (System.DllNotFoundException ex)
            {
                MessageBox.Show("Could not open SQL database.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Fatal("Dll not Found: SQL Open: {0}", ex.Message);
            }
            catch (BadImageFormatException ex)
            {
                logger.Fatal("Wrong Dll Architecture: SQL Open: {0}", ex.Message);
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
                logger.Debug("SQL: Close: {0}", ex.Message);
            }

            if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Closed)
                return true;
            else
                return false;
        }
        
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
                                Id = (int)reader.GetInt32(reader.GetOrdinal("id")),
                                Enabled = (bool)reader["enabled"],
                                DisplayName = (string)reader["DisplayName"],
                                FullPath = (string)reader["fullPath"],
                                Image = new Bitmap(ByteToImage((byte[])reader["image"]))
                            });
                        }
                        catch (System.InvalidCastException ex)
                        {
                            logger.Debug("Invalid Cast: {0}", ex.Message);
                        }
                    }
                }
            }
            return applicationInfos;
        }

        public int AddApplication(ApplicationInfo newApp)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@enabled", newApp.Enabled), new SQLiteParameter("@DisplayName", newApp.DisplayName), new SQLiteParameter("@fullPath", newApp.FullPath), new SQLiteParameter("@image", ImageToByte(newApp.Image)) };
            if (SQL_ExecuteNonQuery("INSERT INTO ApplicationList (enabled,  DisplayName, fullPath, image) values (@enabled, @DisplayName, @fullPath, @image)", parameters) > 0)
            {
                SQLiteDataReader reader = SQL_ExecuteQuery("SELECT * FROM ApplicationList WHERE DisplayName = @DisplayName", parameters);
                if (reader != null)
                {
                    if (reader.VisibleFieldCount > 0)
                    {
                        reader.Read();
                        return (int)reader.GetInt32(reader.GetOrdinal("id"));                            
                    }
                }
            }
            return -1;
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

        public bool UpdateApplication(ApplicationInfo editApp)
        {
            SQLiteParameter[] parameters = { new SQLiteParameter("@id", editApp.Id), new SQLiteParameter("@enabled", editApp.Enabled), new SQLiteParameter("@DisplayName", editApp.DisplayName), new SQLiteParameter("@fullPath", editApp.FullPath), new SQLiteParameter("@image", ImageToByte(editApp.Image)) };
            if (SQL_ExecuteNonQuery("UPDATE ApplicationList SET enabled = @Enabled, DisplayName = @DisplayName, fullPath = @fullPath, image = @image WHERE id = @id", parameters) > 0)
                return true;
            else
                return false;
        }

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
                logger.Debug("SQL: Execute No Query: {0}", ex.Message);
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
                logger.Debug("SQL: Execute No Query: {0}", ex.Message);
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
                logger.Debug("SQL: Execute: {0}", ex.Message);
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
                logger.Debug("SQL: Execute: {0}", ex.Message);
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
    }
}
