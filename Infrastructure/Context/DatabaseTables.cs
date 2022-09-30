using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class DatabaseTables
    {
        public static void SetMappings(ModelBuilder modelBuilder)
        {
            TblUsers.SetMappings(modelBuilder);
        }
    }
}
