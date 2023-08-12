namespace ProjetoPortfolio.API.Models.DTOs.Response
{
    public class HabilidadeResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public string Descricao { get; set; }
        public Guid PessoaId { get; set; }
    }
}
