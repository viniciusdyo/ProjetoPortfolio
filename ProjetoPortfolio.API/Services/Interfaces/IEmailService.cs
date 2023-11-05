using ProjetoPortfolio.API.Models.DTOs;

namespace ProjetoPortfolio.API.Services.Interfaces
{
    public interface IEmailService
    {
        void Enviar(EmailConfigDto emailConfig, string assunto, string html, string de = null);
    }
}
