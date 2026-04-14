using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RazorCrudAppAuth.Models;

namespace RazorCrudAppAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Service> Services { get; set; }
        public DbSet<RepairRequest> RepairRequests { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }

    }
}
