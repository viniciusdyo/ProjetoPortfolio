using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;

namespace ProjetoPortfolio.API.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        Task<AutenticacaoResult> Login(UsuarioLoginRequestDto request);

    }
}
