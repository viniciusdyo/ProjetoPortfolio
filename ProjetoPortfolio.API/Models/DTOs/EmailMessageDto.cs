namespace ProjetoPortfolio.API.Models.DTOs
{
    public class EmailMessageDto
    {
        public string Assunto { get; set; } = string.Empty;
        public string Remetente  { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
    }
}
