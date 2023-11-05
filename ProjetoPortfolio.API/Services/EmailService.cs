using MimeKit;
using MailKit.Security;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Models.DTOs.Response;
using ProjetoPortfolio.API.Repositories.Interfaces;
using ProjetoPortfolio.API.Services.Interfaces;
using System.Data;
using MailKit.Net.Smtp;
using MailKit.Net.Pop3;

namespace ProjetoPortfolio.API.Services
{
    public class EmailService : IEmailService
    {

        public async void Enviar(EmailConfigDto emailConfig, string assunto, string html, string de)
        {

            try
            {
                if (emailConfig == null)
                    throw new ArgumentNullException(nameof(emailConfig));

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(emailConfig.Email.ToString()));
                email.To.Add(MailboxAddress.Parse("viniciusdyo@gmail.com"));
                email.Subject = assunto;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = @$" Email de {de} </br>{html}" };

                using var smtp = new SmtpClient();
                string host = emailConfig.Servidor;
                int porta = emailConfig.Porta;
                smtp.Connect(emailConfig.Servidor.ToString(),Convert.ToInt32(emailConfig.Porta), SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(emailConfig.Email.ToString(),emailConfig.Senha.ToString());
                smtp.Send(email);
                smtp.Disconnect(true);


                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
