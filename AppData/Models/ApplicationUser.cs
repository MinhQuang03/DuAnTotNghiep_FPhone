using System.ComponentModel.DataAnnotations.Schema;
using AppData.ViewModels.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
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
        [NotMapped] 
        public IFormFile Images { get; set; }
        [NotMapped]
        public string? Role { get; set; }

    }
}
