using Microsoft.EntityFrameworkCore;

namespace MVCCoreApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Thing> Things { get; set; }        
    }
}
