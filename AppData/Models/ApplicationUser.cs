using Microsoft.AspNetCore.Identity;

namespace AppData.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? CitizenId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public int Status { get; set; }
    }
}
