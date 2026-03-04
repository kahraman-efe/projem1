namespace WebApplication4.DAL.Entities
{
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public string ImageURL { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }

    }
}