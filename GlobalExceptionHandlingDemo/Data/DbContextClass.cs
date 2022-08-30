using GlobalExceptionHandlingDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace GlobalExceptionHandlingDemo.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Product>? Products { get; set; } 
    }
}
