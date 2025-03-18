using NikeStore.Repository;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new System.Net.NetworkCredential("SWminh0918195615@gmail.com", "dglm kdwe gruc klpy")
        };

        return client.SendMailAsync(
            new MailMessage(from: "SWminh0918195615@gmail.com",
                            to: email,
                            subject,
                            message
                            ));
    }
}
