using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NVidia_Surround_Assistant
{
    public delegate void DelegateCrossThread();

    public partial class ApplicationClosedWaitTimeout : Form
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        int secondsTimeInterval = 0;
        bool timerCanceled = false;
        bool nonUserExit = false;

        public IntPtr hWnd
        {
            get { return base.Handle; }
        }

        public ApplicationClosedWaitTimeout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Is the timer curently active
        /// </summary>
        public bool TimerEnabled
        {
            get
            {
                return (secondsTimeInterval > 0);
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
            nonUserExit = true;
            timerCanceled = true;
            secondTick.Stop();
            this.Close();
        }

        /// <summary>
        /// close the form
        /// </summary>
        public void CloseForm()
        {
            nonUserExit = true;
            timerCanceled = false;
            secondTick.Stop();
            this.Close();
        }

        /// <summary>
        /// Second Tick 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void secondTick_Tick(object sender, EventArgs e)
        {
            secondsTimeInterval--;
            if (secondsTimeInterval > 0)
            {
                labelSeconds.Text = secondsTimeInterval.ToString();
                secondTick.Start();
            }
            else
            {
                CloseForm();
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
            if (!nonUserExit && e.CloseReason == CloseReason.UserClosing)
            {
                timerCanceled = true;
            }
        }

        private void ApplicationClosedWaitTimeout_Shown(object sender, EventArgs e)
        {
            SetForegroundWindow(hWnd);
        }

        private void ApplicationClosedWaitTimeout_Leave(object sender, EventArgs e)
        {
            SetForegroundWindow(hWnd);
        }
    }
}
