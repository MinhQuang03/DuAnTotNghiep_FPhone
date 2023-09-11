namespace AppData.Models
{
    public class ListImage
    {
        public Guid Id { get; set; }

        public string? Image { get; set; }

        public Guid? IdPhoneDetaild { get; set; }

        public Guid? IdColor { get; set; }

        public PhoneDetaild? PhoneDetailds { get; set; }

    }
}
