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
}