using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class TokenProvider
    {
        private static Token token = new()
        {
            TokenValue = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6ImRkYzNkZWM0OTQzMzQyZWMxZGQ0NGUxMmI1OTM4NTEwODQ5NTA2YTRlYzA2YzI4NzFkYjhhNmZjZGI1ZDkwZDBhMzA1ZWFmYzUwMjIyOWNiIn0.eyJhdWQiOiIxN2RjMmM3NS0zNDcyLWFhYjYtYTZiYy02MjQxZmUxNDAxMzAiLCJqdGkiOiJkZGMzZGVjNDk0MzM0MmVjMWRkNDRlMTJiNTkzODUxMDg0OTUwNmE0ZWMwNmMyODcxZGI4YTZmY2RiNWQ5MGQwYTMwNWVhZmM1MDIyMjljYiIsImlhdCI6MTY1Mzk0MzgwOCwibmJmIjoxNjUzOTQzODA4LCJleHAiOjE2NTM5NDc0MDgsInN1YiI6IiIsInNjb3BlcyI6W119.UKjC-72Fp5DHi5yfrjExHkZ0vWYEyjK8WUhQsMcV3aJBlplv28lDvuItUYWugrJUkn7t4Ax_GPYmTSUPlwqiKG7GWvWKFeDNDndPwsB78IrX2Ypn2vE6Hat3gWwzV-Mpf75D2a1f_64bvZ-sF2gd7W0IGF7kEtv8qidSIuxtW68lbjQuin5VI5J-t3wNE9lfuMwr0NB7rtlj7BLdTu9HdRN2ZdUMLBm9EXcTERGZ9ag2wvfTWQBxhX9peHSvkwrgjLZwuOYHXglC8Got64r7SROFb0E9ZwrC9yVRlbNsFSVCq2qrxBek22emjSDqfPsJl6Vekjxmp_dFqW-LJt7LvQ",
            ValidTillDate = DateTime.UtcNow.AddSeconds(3000),
        };

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

            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");
            var response = client.PostAsync("/api/oauth/access_token", stringContent).GetAwaiter().GetResult();

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
