namespace ProjetoPortfolio.API.Models
{
    public class Habilidade
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public string Descricao { get; set; }
        public Guid PessoaId { get; set; }
        public PessoaPortfolio Pessoa  { get; set; }

    }
}
