namespace AppData.Models
{
    public class Rank
    {
        public Guid Id { get; set; }

        public string Username { get; set; }    

        public decimal? Requirement { get; set; }

        public DateTime? DateRank { get; set; }


        public string? Policies { get; set; }
    }
}
