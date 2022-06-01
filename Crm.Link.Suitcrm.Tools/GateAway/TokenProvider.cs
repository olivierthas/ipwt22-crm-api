using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class TokenProvider
    {
        private static Token token = new();

        private readonly ILogger<TokenProvider> logger;
        private readonly HttpClient client;
        private readonly Credentials credentials;

        public TokenProvider(
            ILogger<TokenProvider> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            this.logger = logger;
            client = httpClientFactory.CreateClient("Crm");
            credentials = configuration.GetSection("CrmConfig").Get<Credentials>();
        }

        public string? GetToken()
        {
            if (token.ValidTillDate != null && token.ValidTillDate > DateTime.UtcNow)
                return token.TokenValue;
                        
            var json = JsonConvert.SerializeObject(credentials);
            logger.LogInformation(json);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");
            stringContent.Headers.ContentType.CharSet = "";
            //var response = client.PostAsync("/api/oauth/access_token", stringContent).GetAwaiter().GetResult();
            var response = client.PostAsync("/api/access_token", stringContent).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError(message: "Fetch token failed: ", args: response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                return null;
            }

            var res = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            logger.LogInformation("token response body: {res}", res);
            TokenResponse? content = JsonConvert.DeserializeObject<TokenResponse>(res);
            _ = content ?? throw new ArgumentNullException(nameof(content));

            token.ValidTillDate = DateTime.UtcNow.AddSeconds(content.ExpiresIn);
            token.TokenValue = content?.AccessToken;

            return token.TokenValue;
        }
    }

    public class Token
    {
        public DateTime? ValidTillDate { get; set; } = null;
        public string? TokenValue { get; set; }
    }
}
