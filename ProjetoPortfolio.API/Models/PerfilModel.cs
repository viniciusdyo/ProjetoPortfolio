namespace ProjetoPortfolio.API.Models
{
    public class PerfilModel
    {
        public Guid Id  { get; set; }
        public string Nome  { get; set; }
        public string Descricao { get; set; }
        public string Sobre { get; set; }
        public List<RedeModel> RedesSociais { get; set; }
        public List<HabilidadeModel> Habilidades { get; set; }

    }
    
}
