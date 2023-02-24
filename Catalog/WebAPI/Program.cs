#pragma warning disable ASP0014 // Suggest using top level route registrations

using System.Text.Json;
using CatalogWebAPI.Configurations;
using CatalogWebAPI.Data;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebAPI.Repositories.Implementations;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Implementations;
using WebAPI.Services.Interfaces;

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
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddTransient<ICatalogService, CatalogService>();
            builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();
            builder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();

            builder.Services.AddDbContextFactory<ApplicationDbContext>(options
                => options.UseNpgsql(configuration["ConnectionString"]));

            // Needed for easier maintenance and unit testing (generic is beneficial for unit testing)
            builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>,
                DbContextWrapper<ApplicationDbContext>>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // This route uses the convention of "ControllerName/ActionName/{id?}"
                endpoints.MapDefaultControllerRoute();

                // Maps all other routes for controller action methods.
                endpoints.MapControllers();
            });

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