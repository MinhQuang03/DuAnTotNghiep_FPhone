namespace AppData.Models
{
    public class Imei
    {
        public Guid Id { get; set; }

        public string NameImei { get; set; }

        public int? Status { get; set; }

        public Guid? IdPhoneDetaild { get; set; }

        public virtual PhoneDetaild? PhoneDetailds { get; set; }
    }
}
