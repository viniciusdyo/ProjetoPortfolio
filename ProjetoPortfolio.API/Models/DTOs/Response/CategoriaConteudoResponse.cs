namespace ProjetoPortfolio.API.Models.DTOs.Response
{
    public class CategoriaConteudoResponse
    {
        public Guid CategoriaId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public IEnumerable<ConteudoResponse> Conteudos { get; set; }
    }
}
