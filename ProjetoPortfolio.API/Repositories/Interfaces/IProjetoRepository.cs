using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs.Response;

namespace ProjetoPortfolio.API.Repositories.Interfaces
{
    public interface IProjetoRepository
    {
        Task<PorfolioResponse<ProjetoResponse>> BuscarTodosProjetos();
        Task<PorfolioResponse<ProjetoResponse>> BuscarPorId(Guid id);
        Task<PorfolioResponse<ProjetoResponse>> Adicionar(ProjetoModel projeto);
        Task<PorfolioResponse<ProjetoResponse>> Atualizar(ProjetoModel projeto, Guid id);
        Task<PorfolioResponse<ProjetoResponse>> Apagar(Guid id);
    }
}
