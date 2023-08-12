namespace ProjetoPortfolio.API.Models
{
    public class PessoaPortfolio
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public List<Habilidade> Habilidades { get; set; }
    }
}
