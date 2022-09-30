using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    class TblUsers
    {
        internal static void SetMappings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("tblUsers");
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("UserId");
            modelBuilder.Entity<User>().Property(x => x.Name).IsRequired(true).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.Email).IsRequired(true).HasMaxLength(100);
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique(true);
            modelBuilder.Entity<User>().Property(x => x.Password).IsRequired(true).HasMaxLength(100);
            modelBuilder.Entity<User>().Property(e => e.UserType)
              .HasConversion(
                  x => x.ToString(),
                  x => (UserType)Enum.Parse(typeof(UserType), x));
        }
    }
}
