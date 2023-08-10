using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoPortfolio.API.Models
{
    public class ConteudoModel
    {
        public Guid Id { get; set; }
        public string Conteudo { get; set; }
        public string Titulo { get; set; }
        public string Nome { get; set; }
        public Guid CategoriaConteudoId { get; set; }
        [NotMapped]
        public virtual CategoriaConteudoModel CategoriaConteudoModel { get; set; }
    }
}
