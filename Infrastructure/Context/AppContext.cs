using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DatabaseTables.SetMappings(modelBuilder);
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information);
    }
}
//dotnet-ef migrations add M_tblEmployee_Note_InactivityReason --project Infrastructure --startup-project WebApp
//dotnet-ef migrations remove --project Infrastructure --startup-project WebApp
//dotnet-ef database update --project Infrastructure --startup-project WebApp