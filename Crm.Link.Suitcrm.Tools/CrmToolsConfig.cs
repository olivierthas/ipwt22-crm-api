using Crm.Link.Suitcrm.Tools.GateAway;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Link.Suitcrm.Tools
{
    public static class CrmToolsConfig
    {
        public static IServiceCollection UseCrmTools(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<TokenProvider>();

            services.AddTransient<IAccountGateAway, AccountGateAway>();
            services.AddTransient<ISessionGateAway, SessionGateAway>();
            services.AddTransient<IContactGateAway, ContactGateAway>();

            var uri = configuration.GetConnectionString("crm_url");
            services.AddHttpClient("Crm", client =>
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue { NoCache = true };
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.api+json");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("User-Agent", "Crm.Link.Api");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            });

            return services;
        }
    }
}
