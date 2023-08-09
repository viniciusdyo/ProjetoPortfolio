using ProjetoPortfolio.API.Models;

namespace ProjetoPortfolio.API.Repositories.Interfaces
{
    public interface ICategoriaConteudoRepository
    {
        Task<CategoriaConteudoModel> BuscarPorId(Guid id);
        Task<List<CategoriaConteudoModel>> Listar();
        Task<CategoriaConteudoModel> Adicionar(CategoriaConteudoModel categoria);
        Task<CategoriaConteudoModel> Atualizar(CategoriaConteudoModel categoria);
        Task<CategoriaConteudoModel> Excluir(Guid id);
    }
}
