namespace AppData.Models
{
    public class Rank
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string IdAccount { get; set; }

        public int? Point { get; set; }
        public int? TotalPoint { get; set; }
        public string? Ranking { get; set; }

        public DateOnly? DateRank { get; set; }


        public int? Policies { get; set; }
        public int? Benefits { get; set; }
    }
}
