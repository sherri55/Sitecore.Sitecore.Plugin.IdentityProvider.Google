using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Runtime.Configuration;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Sitecore.Plugin.IdentityProvider.Google.Configuration;

namespace Sitecore.Plugin.IdentityProvider.Google
{
    public class ConfigureSitecore
    {
        private readonly ILogger<ConfigureSitecore> _logger;
        private readonly GoogleAppSettings _googleAppSettings;
        public ConfigureSitecore(ISitecoreConfiguration scConfig, ILogger<ConfigureSitecore> logger)

        {
            this._logger = logger;
            this._googleAppSettings = new GoogleAppSettings();
            scConfig.GetSection(GoogleAppSettings.SectionName);
            scConfig.GetSection(GoogleAppSettings.SectionName).Bind((object)this._googleAppSettings.GoogleIdentityProvider);
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var googleProvider = this._googleAppSettings.GoogleIdentityProvider;

            if (googleProvider.Enabled)

            {
                new AuthenticationBuilder(services).AddOpenIdConnect(googleProvider.AuthenticationScheme, googleProvider.DisplayName, (Action<OpenIdConnectOptions>)(options =>
                {
                    options.SignInScheme = "idsrv.external";

                    options.SignedOutRedirectUri = "https://demo.googleidentityserver.is/Account/Logout";

                    options.ClientId = googleProvider.ClientId;

                    options.ClientSecret = googleProvider.ClientSecret;

                    options.Authority = googleProvider.Authority;

                    options.CallbackPath = googleProvider.CallbackPath;
                    options.Events.OnRedirectToIdentityProvider += (Func<RedirectContext, Task>)(context =>
                    {
                        if (string.Equals(context.HttpContext.User.FindFirst("idp")?.Value, googleProvider.AuthenticationScheme, StringComparison.Ordinal))
                            context.ProtocolMessage.Prompt = "select_account";
                        return Task.CompletedTask;
                    });
                    options.Events.OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        var logoutUri = "https://demo.googleidentityserver.is/Account/Logout";
                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();
                        return Task.CompletedTask;

                    };
                }));
            }
        }
    }
}