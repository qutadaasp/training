using Microsoft.EntityFrameworkCore;

namespace WebApplication6.Models
{
    public class ApplicationDBcontext : DbContext
    {
        public ApplicationDBcontext(DbContextOptions<ApplicationDBcontext> options) : base(options)
        {

        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movies> Movies { get; set; }
    }
}
