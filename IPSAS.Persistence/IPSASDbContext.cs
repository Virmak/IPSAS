using IPSAS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IPSAS.Persistence
{

    public interface IIPSASDbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<MonthlyPayroll> MonthlyPayrolls { get; set; }
        public DbSet<PayrollRecord> PayrollRecords { get; set; }
        public DbSet<Payslip> Payslips { get; set; }

        public DbSet<Payment> Payments { get; set; }
    }

    public class IPSASDbContext: DbContext, IIPSASDbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<MonthlyPayroll> MonthlyPayrolls { get; set; }
        public DbSet<PayrollRecord> PayrollRecords { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;DataBase=ipsas.mdf;Integrated Security=True;Connect Timeout=30");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Teacher>()
                .HasIndex(t => t.CIN)
                .IsUnique(true);

            builder.Entity<PayrollRecord>()
                .HasIndex(pr => new { pr.TeacherId, pr.PayrollId })
                .IsUnique();
        }
    }
}
