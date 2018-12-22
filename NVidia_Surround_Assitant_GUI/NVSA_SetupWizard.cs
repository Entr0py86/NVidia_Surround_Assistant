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
    enum PanelSelect 
    {
        InfoPanel,
        saveDefaultQuestion,
        saveSurroundQuestion,
        saveDefault,
        saveSurround,
        Finish,
    };

    public partial class NVSA_SetupWizard : Form
    {
        PanelSelect currentPanel;
        Stack<PanelSelect> panelHistory = new Stack<PanelSelect>();
        bool questionVisible = false;
        bool questionAnswerYes = false;

        bool defaultSaved = false;
        bool surroundSaved = false;

        public bool SetupSuccessful
        {
            get
            {
                return (defaultSaved & surroundSaved);
            }
        }

        public NVSA_SetupWizard()
        {
            InitializeComponent();
            ShowPanel(PanelSelect.InfoPanel);
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
            if(questionVisible)
            {
                questionAnswerYes = false;
                NextPanel();
            }
            else
            {
                PanelSelect panel = panelHistory.Pop();
                ShowPanel(panel);
            }
        }

        private void pictureBoxForward_Click(object sender, EventArgs e)
        {
            panelHistory.Push(currentPanel);
            if (questionVisible)
            {
                questionAnswerYes = true;
            }
            NextPanel();            
        }

        private void pictureBoxCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Setup Wizard", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
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

        private void ShowPanel(PanelSelect panel)
        {
            switch(panel)
            {
                case PanelSelect.InfoPanel:
                    panelInstructions.Visible = true;                    
                    panelSaveDefaultQuestion.Visible = false;
                    panelSaveSurroundQuestion.Visible = false;
                    panelSaveSurround.Visible = false;
                    panelSaveDefault.Visible = false;
                    panelFinished.Visible = false;
                    break;
                case PanelSelect.saveDefault:
                    panelInstructions.Visible = false;
                    panelSaveDefaultQuestion.Visible = false;
                    panelSaveSurroundQuestion.Visible = false;
                    panelSaveSurround.Visible = false;
                    panelSaveDefault.Visible = true;
                    panelFinished.Visible = false;
                    break;
                case PanelSelect.saveSurround:
                    panelInstructions.Visible = false;
                    panelSaveDefaultQuestion.Visible = false;
                    panelSaveSurroundQuestion.Visible = false;
                    panelSaveSurround.Visible = true;
                    panelSaveDefault.Visible = false;
                    panelFinished.Visible = false;
                    break;
                case PanelSelect.saveDefaultQuestion:
                    panelInstructions.Visible = false;
                    panelSaveDefaultQuestion.Visible = true;
                    panelSaveSurroundQuestion.Visible = false;
                    panelSaveSurround.Visible = false;
                    panelSaveDefault.Visible = false;
                    panelFinished.Visible = false;
                    break;
                case PanelSelect.saveSurroundQuestion:
                    panelInstructions.Visible = false;
                    panelSaveDefaultQuestion.Visible = false;
                    panelSaveSurroundQuestion.Visible = true;
                    panelSaveSurround.Visible = false;
                    panelSaveDefault.Visible = false;
                    panelFinished.Visible = false;
                    break;
                case PanelSelect.Finish:
                    panelInstructions.Visible = false;
                    panelSaveDefaultQuestion.Visible = false;
                    panelSaveSurroundQuestion.Visible = false;
                    panelSaveSurround.Visible = false;
                    panelSaveDefault.Visible = false;
                    panelFinished.Visible = true;
                    break;
            }
            currentPanel = panel;
        }

        private void NextPanel()
        {
            switch (currentPanel)
            {
                case PanelSelect.InfoPanel:
                    if(MainForm.surroundManager.SM_IsDefaultActive())
                    {
                        ShowPanel(PanelSelect.saveDefaultQuestion);
                    }
                    else
                    {
                        ShowPanel(PanelSelect.saveSurroundQuestion);
                    }
                    break;
                case PanelSelect.saveDefault:
                    if(MainForm.sqlInterface.SurroundConfigExists("Default"))
                    {
                        if (MessageBox.Show("Default Profile found. Overwrite it?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            defaultSaved = MainForm.surroundManager.SM_SaveDefaultSetup();
                        else
                            defaultSaved = true;
                    }
                    else
                    {
                        defaultSaved = MainForm.surroundManager.SM_SaveDefaultSetup();
                    }                    
                    if(surroundSaved)
                    {
                        ShowPanel(PanelSelect.Finish);
                    }
                    else
                    {
                        ShowPanel(PanelSelect.saveSurround);
                    }
                    break;
                case PanelSelect.saveSurround:
                    if (MainForm.sqlInterface.SurroundConfigExists("Default Surround"))
                    {
                        if (MessageBox.Show("Default Surround Profile found. Overwrite it?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            surroundSaved = MainForm.surroundManager.SM_SaveDefaultSurroundSetup();
                        else
                            surroundSaved = true;
                    }
                    else
                    {
                        surroundSaved = MainForm.surroundManager.SM_SaveDefaultSurroundSetup();
                    }                    
                    if (defaultSaved)
                    {
                        ShowPanel(PanelSelect.Finish);
                    }
                    else
                    {
                        ShowPanel(PanelSelect.saveDefault);
                    }
                    break;
                case PanelSelect.saveDefaultQuestion:
                    if(questionAnswerYes)
                    {
                        ShowPanel(PanelSelect.saveDefault);
                    }
                    else
                    {
                        ShowPanel(PanelSelect.saveSurround);
                    }
                    break;
                case PanelSelect.saveSurroundQuestion:
                    if (questionAnswerYes)
                    {
                        ShowPanel(PanelSelect.saveSurround);
                    }
                    else
                    {
                        ShowPanel(PanelSelect.saveDefault);
                    }
                    break;
                case PanelSelect.Finish:
                    Close();
                    break;
            }
        }

        private void panelSaveQuestion_VisibleChanged(object sender, EventArgs e)
        {
            if ((sender as Panel).Visible)
            {
                questionVisible = true;
                pictureBoxBack.Image = Properties.Resources.delete_filled_red_24x24;
                pictureBoxForward.Image = Properties.Resources.success_green_24x24;//TODO make bigger images
            }
            else
            {
                questionVisible = false;
                pictureBoxBack.Image = Properties.Resources.back_48x48;
                pictureBoxForward.Image = Properties.Resources.forward_48x48;
            }
        }

        private void panelFinished_VisibleChanged(object sender, EventArgs e)
        {
            if (panelFinished.Visible)
            {
                pictureBoxForward.Image = Properties.Resources.success_green_24x24;//TODO make bigger images
            }
            else
            {
                pictureBoxForward.Image = Properties.Resources.forward_48x48;
            }
        }

        /// <summary>
        /// Override ProcessCmdKey to have the shortcut work
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Enter | Keys.Space))
            {
                pictureBoxForward_Click(null, null);
                return true;
            }
            else if(keyData == Keys.Back)
            {
                pictureBoxBack_Click(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
