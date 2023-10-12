namespace ProjetoPortfolio.API.Models
{
    public class RedeModel
    {
        public Guid Id { get; set; }
        public string Rede { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }
        public PerfilModel Perfil { get; set; }
        public PessoaPortfolio Pessoa { get; set; }
    }
}
