using ProjetoPortfolio.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoPortfolio.API.Models
{
    public class AtivoConteudoModel
    {
        public Guid AtivoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public TipoAtivo TipoAtivo { get; set; }
        public Guid ConteudoModelId { get; set; }
        public ConteudoModel ConteudoModel { get; set; }
    }
}
