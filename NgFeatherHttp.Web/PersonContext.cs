using Microsoft.EntityFrameworkCore;

namespace NgFeatherHttp.Web
{
    public class PersonContext : DbContext
    {
        public DbSet<PersonItem> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Persons");
        }
    }
}
