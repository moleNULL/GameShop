using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MVC.Configurations;
using MVC.Services.Implementations;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.AddConfiguration();

            int sessionCookieLifetime = configuration.GetValue("SessionCookieLifetimeMinutes", 60);
            string? callbackUrl = configuration["CallbackUrl"];
            string? redirectUrl = configuration["RedirectUrl"];
            string? identityUrl = configuration["IdentityUrl"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = identityUrl;
                options.Events.OnRedirectToIdentityProvider = async n =>
                {
                    n.ProtocolMessage.RedirectUri = redirectUrl;
                    await Task.FromResult(0);
                };

                options.SignedOutRedirectUri = callbackUrl!;
                options.ClientId = "mvc_pkce";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;
                options.UsePkce = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("mvc");
            });

            builder.Services.Configure<MvcConfig>(configuration);

            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddTransient<IHttpClientService, HttpClientService>();
            builder.Services.AddTransient<ICatalogService, CatalogService>();
            builder.Services.AddTransient<IBasketService, BasketService>();
            builder.Services.AddTransient<IIdentityParser<ApplicationUser>, IdentityParser>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseCookiePolicy(new CookiePolicyOptions() { MinimumSameSitePolicy = SameSiteMode.Lax });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Catalog}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultError", "{contoller=Error}/{action=Error}");
                endpoints.MapControllers();
            });

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
    }
}