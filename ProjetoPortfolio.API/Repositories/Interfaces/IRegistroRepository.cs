using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;

namespace ProjetoPortfolio.API.Repositories.Interfaces
{
    public interface IRegistroRepository
    {
        Task<AutenticacaoResult> Registrar(UsuarioRegistroRequestDto request);
    }
}
