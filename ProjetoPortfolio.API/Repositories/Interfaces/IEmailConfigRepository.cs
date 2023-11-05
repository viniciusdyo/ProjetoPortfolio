using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Models.DTOs.Response;

namespace ProjetoPortfolio.API.Repositories.Interfaces
{
    public interface IEmailConfigRepository
    {
        Task<PorfolioResponse<EmailConfigModel>> Cadastrar(EmailConfigDto emailConfig);
        Task<PorfolioResponse<EmailConfigModel>> Remover(int id);
        Task<PorfolioResponse<EmailConfigModel>> Editar(int id, EmailConfigDto emailConfig);
        Task<PorfolioResponse<EmailConfigDto>> BuscarPorId(int id);
    }
}
