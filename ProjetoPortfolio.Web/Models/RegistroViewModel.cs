
using System.ComponentModel.DataAnnotations;

namespace ProjetoPortfolio.Web.Models
{
    public class RegistroViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "O nome de usuário precisa ter pelo menos {2} caracteres.", MinimumLength = 6)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Formato de E-mail inválido")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
