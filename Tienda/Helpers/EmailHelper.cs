using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static void EnviarEmail2(string to, string subject, StringReader a,string body, string from)
        {
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();
            htmlparser.Parse(a);
            pdfDoc.Close();
            byte[] bytes = memoryStream.ToArray();
       


            var message = new System.Net.Mail.MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(from);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment(new MemoryStream(bytes), "comprobante.pdf"));
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
                smtp.Send(message);
            }
        }

    }
}