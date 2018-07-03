using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NVidia_Surround_Assistant
{
    public partial class SurroundConfigSaveAsPopup : Form
    {
        public String surroundConfigName { get; set; }

        public SurroundConfigSaveAsPopup()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            surroundConfigName = textBoxSurroundConfigName.Text;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            surroundConfigName = "";
            DialogResult = DialogResult.Cancel;

            Close();
        }
    }
}
