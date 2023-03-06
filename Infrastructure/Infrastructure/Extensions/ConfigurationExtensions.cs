using Infrastructure.Configuration;

namespace Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            var clientSection = builder.Configuration.GetSection("Client");
            builder.Services.Configure<ClientConfig>(clientSection);

            var authorizationSection = builder.Configuration.GetSection("Authorization");
            builder.Services.Configure<AuthorizationConfig>(authorizationSection);
        }
    }
}
