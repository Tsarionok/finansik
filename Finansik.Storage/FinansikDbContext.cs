using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage;

public class FinansikDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Group> Groups { get; set; }
    
    public DbSet<Period> Periods { get; set; }
    
    public DbSet<PeriodCategory> PeriodCategories { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<PerformedOperation> PerformedOperations { get; set; }
    
    public DbSet<ScheduledOperation> ScheduledOperations { get; set; }
}