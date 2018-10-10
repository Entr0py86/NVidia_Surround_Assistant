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
    public partial class ApplicationClosedWaitTimeout : Form
    {
        int secondsTimeInterval = 0;
        bool timerCanceled = false;
        System.Timers.Timer secondTick = new System.Timers.Timer();
        

        public ApplicationClosedWaitTimeout()
        {
            InitializeComponent();

            //Setup process stopped timer. 
            secondTick.Interval = 1000;//1seconds
            secondTick.AutoReset = true;
            secondTick.Elapsed += secondTick_Tick;           
        }

        /// <summary>
        /// Is the timer curently active
        /// </summary>
        public bool TimerEnabled
        {
            get
            {
                return secondTick.Enabled;
            }
        }

        /// <summary>
        /// Was the timer canceld by the user
        /// </summary>
        public bool TimerCanceled
        {
            get
            {
                return timerCanceled;
            }
        }

        /// <summary>
        /// Set and Start the timer 
        /// </summary>
        /// <param name="SecondsTimeInterval"></param>
        public void StartTimer(int SecondsTimeInterval)
        {
            secondsTimeInterval = SecondsTimeInterval;
            labelSeconds.Text = secondsTimeInterval.ToString();
            secondTick.Start();
        }

        /// <summary>
        /// Cancel the timer and close the form
        /// </summary>
        public void CancelTimerAndClose()
        {
            timerCanceled = true;
            secondTick.Stop();
            this.Close();
        }

        /// <summary>
        /// Second Tick 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void secondTick_Tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            secondsTimeInterval--;
            if(secondsTimeInterval > 0)
            {
                labelSeconds.Text = secondsTimeInterval.ToString();
            }
            else
            {
                timerCanceled = false;
                this.Close();
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
            if (keyData == (Keys.Control | Keys.Z))
            {
                CancelTimerAndClose();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ApplicationClosedWaitTimeout_FormClosing(object sender, FormClosingEventArgs e)
        {
            //If user is closing the form then cancel the timer.
            if(e.CloseReason == CloseReason.UserClosing)
            {
                timerCanceled = true;
            }
        }        
    }
}
