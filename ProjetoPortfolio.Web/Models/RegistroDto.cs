using System.ComponentModel.DataAnnotations;

namespace ProjetoPortfolio.Web.Models
{
    public class RegistroDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
