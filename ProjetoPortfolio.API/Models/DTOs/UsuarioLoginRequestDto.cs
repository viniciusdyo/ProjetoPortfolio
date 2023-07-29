using System.ComponentModel.DataAnnotations;

namespace ProjetoPortfolio.API.Models.DTOs
{
    public class UsuarioLoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
