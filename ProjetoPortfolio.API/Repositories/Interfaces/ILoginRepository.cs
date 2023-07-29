using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models.DTOs;

namespace ProjetoPortfolio.API.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        Task<UsuarioLoginRequestDto> Login(UsuarioLoginRequestDto request);

    }
}
