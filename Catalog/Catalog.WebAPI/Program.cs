using CatalogWebAPI.Configurations;
using CatalogWebAPI.Data;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(HttpGlobalValidationActionFilter));
                options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            })
            .AddJsonOptions(jsoptions =>
            {
                jsoptions.JsonSerializerOptions.WriteIndented = true;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.AddConfiguration();
            builder.Services.Configure<CatalogConfig>(configuration);
            builder.Services.AddAuthorization(configuration);

            builder.Services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SwaggerAnnotation.xml"));

                options.SwaggerDoc(name: "v1", info: new OpenApiInfo
                {
                    Title = "GameShop - Catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog HTTP API for GameShop"
                });

                string? authority = configuration["Authorization:Authority"];
                options.AddSecurityDefinition(name: "oauth2", securityScheme: new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                            TokenUrl = new Uri($"{authority}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "mvc", "website" },
                                { "catalog.catalogbff", "catalog.catalogbff" },
                                { "catalog.catalogitem", "catalog.catalogitem" }
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

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
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Catalog.API V1");
                setup.OAuthClientId("catalogswaggerui");
                setup.OAuthAppName("Catalog Swagger UI");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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