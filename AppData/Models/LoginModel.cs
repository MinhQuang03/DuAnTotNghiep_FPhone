using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class LoginModel
    {
        //    [EmailAddress]
        //    public string Email { get; set; }     
        [Required]
        public string UserName { get; set; } = null!;
        [PasswordPropertyText,Required]
        public string Password { get; set; } = null!;

    }
}
