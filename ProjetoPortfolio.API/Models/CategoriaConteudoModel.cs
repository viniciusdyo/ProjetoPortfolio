using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoPortfolio.API.Models
{
    public class CategoriaConteudoModel
    {
        public Guid CategoriaId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public ICollection<ConteudoModel> ConteudoModels { get; set; }
    }
}
