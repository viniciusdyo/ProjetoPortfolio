using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models.DTOs.Response;

namespace ProjetoPortfolio.API.Repositories.Interfaces
{
    public interface IPessoaRepository
    {
        Task<PorfolioResponse<PessoaResponse>> Listar();
        Task<PorfolioResponse<PessoaResponse>> BuscarPorId(Guid id);
        Task<PorfolioResponse<PessoaResponse>> Atualizar(Guid id, PessoaResponse pessoa);
        Task<PorfolioResponse<PessoaResponse>> Cadastrar(PessoaResponse pessoa);
        Task<PorfolioResponse<PessoaResponse>> Remover(Guid id);
    }
}
