using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;

namespace ProjetoPortfolio.API.Repositories.Interfaces
{
    public interface ICategoriaConteudoRepository
    {
        Task<CategoriaConteudoModel> BuscarPorId(Guid id);
        Task<List<CategoriaConteudoModel>> Listar();
        Task<CategoriaConteudoModel> Adicionar(CategoriaConteudoDto categoria);
        Task<CategoriaConteudoModel> Atualizar(CategoriaConteudoDto categoria);
        Task<CategoriaConteudoModel> Excluir(Guid id);
    }
}
