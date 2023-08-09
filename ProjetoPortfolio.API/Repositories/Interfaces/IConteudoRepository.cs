using ProjetoPortfolio.API.Models;

namespace ProjetoPortfolio.API.Repositories.Interfaces;

    public interface IConteudoRepository
    {
    Task<ConteudoModel> BuscarPorId(Guid id);
    Task<List<ConteudoModel>> Listar();
    Task<ConteudoModel> Adicionar(ConteudoModel conteudo);
    Task<ConteudoModel> Atualizar(ConteudoModel conteudo);
    Task<ConteudoModel> Excluir(Guid id);
}

