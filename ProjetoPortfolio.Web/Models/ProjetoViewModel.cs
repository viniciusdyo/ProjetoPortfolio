using ProjetoPortfolio.Entities.Enums;
using System.Text.RegularExpressions;

namespace ProjetoPortfolio.Web.Models
{
    public class ProjetoViewModel
    {
        public Guid Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public StatusProjeto Status { get; set; }
        public string? UrlImagem { get; set; }
        public string? UrlRedirecionar { get; set; }
        public bool Excluido { get; set; }
        public string? TituloNormalizado { get; set; }
        
    }

}
