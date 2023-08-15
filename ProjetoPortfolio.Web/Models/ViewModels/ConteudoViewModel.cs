
namespace ProjetoPortfolio.Web.Models.ViewModels
{
    public class ConteudoViewModel
    {
        public CategoriaConteudo CategoriaConteudo { get; set; }
        public ConteudoModel Conteudo { get; set; }
        public List<CategoriaConteudo> Categorias { get; set; }
        public List<ConteudoModel> Conteudos { get; set; }
    }
}
