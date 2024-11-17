using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppNet2.Models
{
    public class DbNet2Context : IdentityDbContext<ApplicationUser>
    {
        public DbNet2Context(DbContextOptions<DbNet2Context> options) : base(options)
        {
        }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}
