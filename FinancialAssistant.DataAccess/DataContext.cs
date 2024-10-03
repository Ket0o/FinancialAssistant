using FinancialAssistant.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistant.DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<PasswordResetCode> PasswordResetCodes  { get; set; } = null!;
}