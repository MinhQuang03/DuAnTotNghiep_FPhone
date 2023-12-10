namespace AppData.Models
{
    public class ListImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Image { get; set; }

        public Guid? IdPhoneDetaild { get; set; }

        public Guid? IdColor { get; set; } = Guid.NewGuid();

        public PhoneDetaild? PhoneDetailds { get; set; }

    }
}
