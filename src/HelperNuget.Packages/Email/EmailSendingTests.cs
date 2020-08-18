using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Helper.Nuget.Packages.Email
{
    /* Nuget Dependency Package: MailKit */

    /// <summary>
    /// If you use gmail account please check you allow your gmail from account to send email.
    /// For security reason google does not allow to send email from your from address. You may explicity allow this.
    /// </summary>
    public class EmailSendingTests
    {
        /// <summary>
        /// If not exception has thrown consider email sending pass. You can check your inbox then.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldSendEmail()
        {
            using var client = new SmtpClient
            {
                Port = 587,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(
                    "a@gmail.com",
                     "??????????????"
                )
            };

            var mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = "Hello Test",
                Body = "Test purpose only",
            };

            mail.From = new MailAddress("bbbbb@gmail.com", "no_replay@ayz.ch");
            mail.To.Add("aaa@gmail.com");

            await client.SendMailAsync(mail);
        }
    }
}
