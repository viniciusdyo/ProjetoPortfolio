using ProjetoPortfolio.API.Models;

namespace ProjetoPortfolio.API.Repositories.Interfaces
{
    public interface IProjetoRepository
    {
        Task<List<ProjetoModel>> BuscarTodosProjetos();
        Task<ProjetoModel> BuscarPorId(Guid id);
        Task<ProjetoModel> Adicionar(ProjetoModel projeto);
        Task<ProjetoModel> Atualizar(ProjetoModel projeto, Guid id);
        Task<bool> Apagar(Guid id);
    }
}
