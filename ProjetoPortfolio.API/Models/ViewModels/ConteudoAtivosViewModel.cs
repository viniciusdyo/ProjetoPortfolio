using ProjetoPortfolio.API.Models.DTOs;

namespace ProjetoPortfolio.API.Models.ViewModels
{
    public class ConteudoAtivosViewModel
    {
        public ConteudoDto Conteudo { get; set; }
        public List<AtivoConteudoDto>? Ativos { get; set; } = new List<AtivoConteudoDto>();
    }
}
