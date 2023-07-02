using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BaoDatShop.DTO.Voucher;

namespace BaoDatShop.DTO.AccountRequest
{
    public interface IEmailSender
    {
        void SendEmailAsync(SendVoucher model);
    }
    public class EmailSender : IEmailSender
    {
        public void SendEmailAsync(SendVoucher model)
        {
            //var client = new SmtpClient("smtp.office365.com", 587)
            //{
            //    EnableSsl = true,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential("10video10a10@gmail.com", "inuyasa951")
            //};

            //return client.SendMailAsync(
            //    new MailMessage(from: "10video10a10@gmail.com",
            //                    to: email,
            //                    subject,
            //                    message
            //                    ));
            string frommail = "10video10a10@gmail.com";
            string pass = "upyyyjmlbqdykyvs";
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = model.subject;
            mailMessage.Body ="Bạn đã nhận được Voucher giảm giá, hãy nhập Voucher này để nhận được ưu đãi " + model.message;
            mailMessage.To.Add(new MailAddress(model.email));
            mailMessage.From=new MailAddress(frommail);
         
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(frommail, pass)
            };
             client.Send(mailMessage);

        }
    }
}
