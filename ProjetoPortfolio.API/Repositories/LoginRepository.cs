using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        public Task<UsuarioLoginRequestDto> Login(UsuarioLoginRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
