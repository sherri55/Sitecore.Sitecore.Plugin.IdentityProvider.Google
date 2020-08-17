namespace Sitecore.Plugin.IdentityProvider.Google.Configuration
{
    class GoogleIdentityProvider : Sitecore.Plugin.IdentityProviders.IdentityProvider
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
        public string CallbackPath { get; set; }

    }
}
