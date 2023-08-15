using ProjetoPortfolio.Entities.Enums;

namespace ProjetoPortfolio.Web.Models.Response
{
    public class AtivoResponse
    {
        public Guid AtivoId { get; set; }
        public string NomeAtivo { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public TipoAtivo TipoAtivo { get; set; }
        public Guid ConteudoModelId { get; set; }
    }
}
