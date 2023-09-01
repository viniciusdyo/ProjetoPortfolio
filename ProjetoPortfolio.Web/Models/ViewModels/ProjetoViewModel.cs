using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.Entities.Enums;
using System.Text.RegularExpressions;

namespace ProjetoPortfolio.Web.Models.ViewModels
{
    public class ProjetoViewModel
    {
        public List<ProjetoModel>? Projetos { get; set; }
        public List<string>? Erros { get; set; }
    }

}
