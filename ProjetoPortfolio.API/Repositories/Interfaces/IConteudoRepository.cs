using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;

namespace ProjetoPortfolio.API.Repositories.Interfaces;

    public interface IConteudoRepository
    {
    Task<ConteudoModel> BuscarPorId(Guid id);
    Task<List<ConteudoModel>> Listar();
    Task<bool> Adicionar(ConteudoDto conteudo, List<AtivoConteudoDto> ativos);
    Task<ConteudoModel> Atualizar(ConteudoDto conteudo, List<AtivoConteudoDto> ativos);
    Task<ConteudoModel> Excluir(Guid id);
}

