using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class SignUpModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText, Required]
        public string Password { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CitizenId { get; set; }
        public int Status { get; set; }
        public string ImageUrl { get; set; }
    }
}
