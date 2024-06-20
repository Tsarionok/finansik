using Finansik.Common;
using Finansik.Storage;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dataSourceBuilder = new NpgsqlDataSourceBuilder("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=finansik;Connection Lifetime=0;");
dataSourceBuilder.MapEnum<OperationDirection>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContextPool<FinansikDbContext>(options => options.UseNpgsql(dataSource));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();