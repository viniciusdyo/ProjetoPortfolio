namespace ProjetoPortfolio.API.Models.DTOs.Response
{
    public class PessoaResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public IEnumerable<HabilidadeResponse> Habilidades { get; set; }
    }
}
