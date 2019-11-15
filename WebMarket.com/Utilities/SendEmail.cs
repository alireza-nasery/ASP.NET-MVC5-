using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebMarket.com
{
    public static class SendEmail
    {
        public static void Send(string To, string Subject, string Body)
        {
            //MailMessage email = new MailMessage("webmarket@api-auth-crack.ir", To, Subject, Body);
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("webmail.api-auth-crack.ir");
            mail.From = new MailAddress("webmarket@api-auth-crack.ir", "وب مارکت");
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);
            SmtpServer.Port = 25;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("alirezanaseri3380@gmail.com", "Lop1010&^&pp0");
            SmtpServer.Credentials = new System.Net.NetworkCredential("webmarket@api-auth-crack.ir", "2$i9gt4Z");
            SmtpServer.EnableSsl = false;

            SmtpServer.Send(mail);

        }
    }
}