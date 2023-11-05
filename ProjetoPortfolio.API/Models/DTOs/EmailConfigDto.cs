namespace ProjetoPortfolio.API.Models.DTOs
{
    public class EmailConfigDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Servidor { get; set; }
        public int Porta { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
