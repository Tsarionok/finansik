using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Testcontainers.PostgreSql;

namespace Finansik.Storage.Tests;

public class StorageTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();

    public FinansikDbContext GetDbContext() => new(new DbContextOptionsBuilder<FinansikDbContext>()
        .UseNpgsql(_dbContainer.GetConnectionString()).Options);

    public IMapper GetMapper() =>
        new Mapper(new MapperConfiguration(cfg =>
            cfg.AddMaps(Assembly.GetAssembly(typeof(FinansikDbContext)))));

    public IMemoryCache GetMemoryCache() => new MemoryCache(new MemoryCacheOptions());
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var dbContext = new FinansikDbContext(new DbContextOptionsBuilder<FinansikDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString()).Options);
        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}