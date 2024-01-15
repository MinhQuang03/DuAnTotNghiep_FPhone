using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Account
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; }

        public string Password { get; set; }

        public string? Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string? ImageUrl { get; set; }

        public int Status { get; set; }

        public int? Points { get; set; }

        public virtual Cart? Carts { get; set; }
    }
}
