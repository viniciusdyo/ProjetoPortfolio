using ProjetoPortfolio.Entities.Enums;

namespace ProjetoPortfolio.API.Models.DTOs
{
    public class AtivoConteudoDto
    {
        public Guid AtivoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public TipoAtivo TipoAtivo { get; set; }
        public Guid ConteudoModelId { get; set; }
    }
}
