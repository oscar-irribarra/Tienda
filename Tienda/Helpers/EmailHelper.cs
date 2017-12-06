using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Tienda.Models;

namespace Tienda.Helpers
{
    public class EmailHelper
    {
        public static async Task EnviarEmail(string to, string subject, string body, string from)
        {
            var message = new System.Net.Mail.MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(from);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = from,
                    Password = DatosCorreo.pass
                };

                smtp.Credentials = credential;
                smtp.Host = DatosCorreo.host;
                smtp.Port = DatosCorreo.port;
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);
            }
        }
    }
}