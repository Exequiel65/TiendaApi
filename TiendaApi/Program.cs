using AspNetCoreRateLimit;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TiendaApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());
builder.Services.ConfigureRateLimitiong();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureCors();
builder.Services.AddAplicacionServices();
var connectionString = "server=localhost; port=3306; database=TiendaApi; user=root; password=; Persist Security Info=False; Connect Timeout=300";

var serverVersion = new MySqlServerVersion(new Version(10, 4, 27));

//Configuramos la conexión a MySql
builder.Services.AddDbContext<ApplicationDbContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString,serverVersion, b => b.MigrationsAssembly("Entities"))
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );



builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader= true;
    options.ReturnHttpNotAcceptable= true;
}).AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseIpRateLimiting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await TiendaContextSeed.SeedAsync(context, loggerFactory);
        await TiendaContextSeed.SeedRolesAsync(context, loggerFactory);
    }
    catch (Exception exception)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(exception, "Ocurrio un error durante la migracion");

        throw;
    }
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
