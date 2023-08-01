
using System.ComponentModel.DataAnnotations;

namespace ProjetoPortfolio.Web.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Preenchimento de campo obrigatório *")]
        [StringLength(100, ErrorMessage = "O nome de usuário precisa ter pelo menos {2} caracteres.", MinimumLength = 6)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Preenchimento de campo obrigatório *")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Formato de E-mail inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Preenchimento de campo obrigatório *")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Preenchimento de campo obrigatório *")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação precisam ser iguais.")]
        public string ConfirmPassword { get; set; }


    }
}
