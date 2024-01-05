using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AppData.IServices;
using AppData.Utilities;

namespace AppData.Services
{
    public class EmailService :IEmailService
    {
        public async Task<ObjectEmailOutput> SendEmail(ObjectEmailInput obj) 
        {
            var output = new ObjectEmailOutput();
            string Address = "fphone.store.404@gmail.com"; // Your email address
            string Password = "bdrczcwdttczwbsv"; // App password

            using (var smtp = new SmtpClient())
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(Address, Password);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(Address),
                    Subject = obj.Subject,
                    Body = obj.Message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(obj.SendTo);

                try
                {
                    await smtp.SendMailAsync(mailMessage);
                    output.Success = true;
                    output.Message = "Email sent successfully";
                    return output;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }

            output.Success = false;
            output.Message = "Failed to send email";
            return output;
        }
    }
}
