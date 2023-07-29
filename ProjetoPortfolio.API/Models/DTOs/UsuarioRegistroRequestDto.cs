using System.ComponentModel.DataAnnotations;

namespace ProjetoPortfolio.API.Models.DTOs
{
    public class UsuarioRegistroRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
