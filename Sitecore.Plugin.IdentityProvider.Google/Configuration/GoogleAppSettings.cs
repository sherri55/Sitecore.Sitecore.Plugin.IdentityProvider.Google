namespace Sitecore.Plugin.IdentityProvider.Google.Configuration
{
    class GoogleAppSettings
    {
            public static readonly string SectionName = "Sitecore:ExternalIdentityProviders:IdentityProviders:Google";
            public GoogleIdentityProvider GoogleIdentityProvider { get; set; } = new GoogleIdentityProvider();     
    }
}
