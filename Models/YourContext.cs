using Microsoft.EntityFrameworkCore;
 
namespace SiteTracker.Models
{
    public class YourContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public YourContext(DbContextOptions<YourContext> options) : base(options) { }
      
        //<Reviewer> is the class model that will link to the database
        public DbSet<Site> Sites{ get; set; }

        public DbSet<Admin> Admins{ get; set; }
        



    }
}