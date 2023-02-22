using CatalogWebAPI.Configurations;
using CatalogWebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = GetConfiguration();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Bind data from appsettings.json to CatalogConfig properties
            builder.Services.Configure<CatalogConfig>(configuration);
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddDbContextFactory<ApplicationDbContext>(options
                => options.UseNpgsql(configuration["ConnectionString"]));

            var app = builder.Build();

            await CreateDbIfNotExistsAsync(app);

            app.Run();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            
            return builder.Build();
        }

        private static async Task CreateDbIfNotExistsAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();
                    await DbInitializer.SeedAsync(dbContext);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured while creating the database");
                }
            }
        }
    }
}