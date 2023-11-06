using MimeKit;
using MailKit.Security;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Models.DTOs.Response;
using ProjetoPortfolio.API.Repositories.Interfaces;
using ProjetoPortfolio.API.Services.Interfaces;
using System.Data;
using MailKit.Net.Smtp;
using MailKit.Net.Pop3;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;

namespace ProjetoPortfolio.API.Services
{
    public class EmailService : IEmailService
    {

        public PorfolioResponse<EmailMessageDto> Enviar(EmailConfigDto emailConfig, EmailMessageDto mensagem)
        {

            try
            {
                if (emailConfig == null)
                    throw new ArgumentNullException(nameof(emailConfig));

                if (mensagem == null)
                    throw new ArgumentNullException("Mensagem nula");

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(emailConfig.Email.ToString()));
                email.To.Add(MailboxAddress.Parse("viniciusdyo@gmail.com"));
                email.Subject = mensagem.Assunto;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = @$" Email de {mensagem.Remetente} </br>{mensagem.Mensagem}" };

                using var smtp = new SmtpClient();
                string host = emailConfig.Servidor;
                int porta = emailConfig.Porta;
                smtp.Connect(emailConfig.Servidor.ToString(), Convert.ToInt32(emailConfig.Porta), SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(emailConfig.Email.ToString(), emailConfig.Senha.ToString());
                smtp.Send(email);
                smtp.Disconnect(true);

                PorfolioResponse<EmailMessageDto> response = new()
                {
                    Results = new List<EmailMessageDto>()
                    {
                        mensagem
                    },
                    Errors = new List<string>()
                };

                return response;
            }
            catch (Exception ex)
            {
                PorfolioResponse<EmailMessageDto> response = new()
                {
                    Results = new List<EmailMessageDto>(),
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                };

                return response;
            }
        }

    }
}
