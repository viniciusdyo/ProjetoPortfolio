namespace ProjetoPortfolio.API.Models
{
    public class EmailConfigModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Servidor { get; set; }
        public int Porta { get; set; }
        public string Email { get; set; }
        public byte[] Senha { get; set; }
    }
}
