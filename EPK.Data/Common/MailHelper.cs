using System;
using System.Net.Mail;

namespace EPK.Data.Common
{
    public class MailHelper
    {
        public static bool SendMail(string toEmail, string subject, string content, string filename)
        {
            try
            {
                var host = ConfigHelper.GetByKey("SMTPHost");
                var port = int.Parse(ConfigHelper.GetByKey("SMTPPort"));
                var fromEmail = ConfigHelper.GetByKey("FromEmailAddress");
                var password = ConfigHelper.GetByKey("FromEmailPassword");
                var fromName = ConfigHelper.GetByKey("FromName");

                var smtpClient = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromEmail, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Timeout = 100000
                };

                var mail = new MailMessage
                {
                    Body = content,
                    Subject = subject,
                    From = new MailAddress(fromEmail, fromName)
                };

                mail.To.Add(new MailAddress(toEmail));

                if (!string.IsNullOrEmpty(filename))
                {
                    Attachment attachment;
                    attachment = new Attachment(filename);
                    attachment.Name = "Baocaochitiet" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                    mail.Attachments.Add(attachment);
                }

                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}