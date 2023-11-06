using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Models.DTOs.Response;

namespace ProjetoPortfolio.API.Services.Interfaces
{
    public interface IEmailService
    {
        PorfolioResponse<EmailMessageDto> Enviar(EmailConfigDto emailConfig, EmailMessageDto mensagem);
    }
}
