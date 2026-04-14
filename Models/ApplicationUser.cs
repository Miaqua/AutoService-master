using Microsoft.AspNetCore.Identity;

namespace RazorCrudAppAuth.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }         
        public string Role { get; set; }             
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public ICollection<RepairRequest> RepairRequests { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

}
