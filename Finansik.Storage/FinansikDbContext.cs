using Finansik.Domain.Models;
using Finansik.Storage.Entities;
using Finansik.Storage.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Category = Finansik.Storage.Entities.Category;
using Group = Finansik.Storage.Entities.Group;
using ScheduledOperation = Finansik.Storage.Entities.ScheduledOperation;

namespace Finansik.Storage;

public class FinansikDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Group> Groups { get; set; }
    
    public DbSet<Period> Periods { get; set; }
    
    public DbSet<PeriodCategory> PeriodCategories { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<PerformedOperation> PerformedOperations { get; set; }
    
    public DbSet<ScheduledOperation> ScheduledOperations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
        => builder.HasPostgresEnum<OperationDirection>();
}