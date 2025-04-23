using Microsoft.AspNetCore.Identity;

namespace EC2_2100212.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }

        public Gender Gender { get; set; }
        public string? Image { get; set; }
    }
}
