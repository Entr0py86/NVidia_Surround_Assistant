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
    public partial class NVSA_SetupWizard : Form
    {
        //TODO previosPanel
        //Calc next panel based on current

        public NVSA_SetupWizard()
        {
            InitializeComponent();
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackColor = MainForm.hoverButtonColor;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackColor = MainForm.normalControlColor;
        }

        private void pictureBoxBack_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxForward_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelInstructions_VisibleChanged(object sender, EventArgs e)
        {
            if(panelInstructions.Visible == true)
            {
                pictureBoxBack.Visible = false;
            }
            else
            {
                pictureBoxBack.Visible = true;
            }
        }
    }
}
