
namespace ProjetoPortfolio.Web.Models.ViewModels
{
    public class ConteudoViewModel
    {
        public CategoriaConteudoModel CategoriaConteudo { get; set; }
        public ConteudoModel Conteudo { get; set; }
        public List<CategoriaConteudoModel> Categorias { get; set; }
    }
}
