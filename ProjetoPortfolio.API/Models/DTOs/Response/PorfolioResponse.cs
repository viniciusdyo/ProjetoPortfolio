namespace ProjetoPortfolio.API.Models.DTOs.Response
{
    public class PorfolioResponse<T>
    {
        public PorfolioResponse()
        {

        }
        public PorfolioResponse(IEnumerable<T> results) => Results = results ?? Array.Empty<T>();
        public IEnumerable<T> Results { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
