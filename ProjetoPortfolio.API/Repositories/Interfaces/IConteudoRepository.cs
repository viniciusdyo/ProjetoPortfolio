using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Models.DTOs.Response;

namespace ProjetoPortfolio.API.Repositories.Interfaces;

    public interface IConteudoRepository
    {
    Task<PorfolioResponse<ConteudoResponse>> BuscarPorId(Guid id);
    Task<PorfolioResponse<ConteudoResponse>> Listar();
    Task<PorfolioResponse<ConteudoResponse>> Adicionar(ConteudoDto conteudo, List<AtivoConteudoDto> ativos);
    Task<PorfolioResponse<ConteudoResponse>> Atualizar(ConteudoDto conteudo, List<AtivoConteudoDto> ativos);
    Task<PorfolioResponse<ConteudoResponse>> Excluir(Guid id);
}

