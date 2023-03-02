namespace MVC.Configurations
{
    public class MvcConfig
    {
        public string CatalogUrl { get; set; } = null!;
        public string BasketUrl { get; set; } = null!;
        public int SessionCookieLifetimeMinutes { get; set; }
        public string CallbackUrl { get; set; } = null!;
        public string IdentityUrl { get; set; } = null!;
    }
}
