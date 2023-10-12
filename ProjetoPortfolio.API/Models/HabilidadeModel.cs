namespace ProjetoPortfolio.API.Models
{
    public class HabilidadeModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public string Descricao { get; set; }
        public Guid PessoaId { get; set; }
        public PessoaPortfolio Pessoa  { get; set; }
        public PerfilModel Perfil { get; set; }
    }
}
