namespace ProjetoPortfolio.Web.Models.Response
{
    public class PorfolioResponse<T>
    {
        public PorfolioResponse()
        {

        }
        public PorfolioResponse(IEnumerable<T> results, List<string> erros)
        {
            Results = results ?? Array.Empty<T>();
            Errors = erros ?? new List<string>();
        }
        public IEnumerable<T> Results { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
