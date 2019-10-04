using IPSAS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IPSAS.Persistence
{
    public class IPSASDbContext: DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ipsas.db;");
        }
    }
}
