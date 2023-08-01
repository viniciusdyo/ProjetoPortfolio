using Microsoft.AspNetCore.Identity;
using ProjetoPortfolio.API.Models.DTOs;

namespace ProjetoPortfolio.API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(IdentityUser user);
    }
}
