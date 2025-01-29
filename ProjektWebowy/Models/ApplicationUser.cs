using Microsoft.AspNetCore.Identity;

namespace ProjektWebowy.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
