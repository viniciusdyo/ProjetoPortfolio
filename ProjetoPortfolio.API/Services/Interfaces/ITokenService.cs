using Microsoft.AspNetCore.Identity;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;

namespace ProjetoPortfolio.API.Services.Interfaces
{
    public interface ITokenService
    {
        Task<AutenticacaoResult> GenerateToken(IdentityUser user);
        Task<AutenticacaoResult> VerificaEGeraToken(TokenRequest request);
        
    }
}
