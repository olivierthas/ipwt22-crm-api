namespace Crm.Link.Api.Configuration.Httpclient
{
    public static class ConfigureHttpClient
    {
        public static IServiceCollection AddHttpClientFactory(this IServiceCollection service, IConfiguration configuration)
        {
            var uri = configuration.GetConnectionString("crm_url");
            service.AddHttpClient("Crm", client =>
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue { NoCache = true };
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.api+json");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("User-Agent", "Crm.Link.Api");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            });

            return service;
        }
    }
}
