using System.Text;
using IdentityModel.Client;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class InternalHttpClientService : IInternalHttpClientService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly AuthorizationConfig _authConfig;
        private readonly ClientConfig _clientConfig;
        private readonly ILogger<InternalHttpClientService> _logger;

        public InternalHttpClientService(
        IHttpClientFactory clientFactory,
        IOptions<ClientConfig> clientConfig,
        IOptions<AuthorizationConfig> authConfig,
        ILogger<InternalHttpClientService> logger)
        {
            _clientFactory = clientFactory;
            _authConfig = authConfig.Value;
            _clientConfig = clientConfig.Value;
            _logger = logger;
        }

        public async Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content)
        {
            var client = _clientFactory.CreateClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = $"{_authConfig.Authority}/connect/token",

                ClientId = _clientConfig.Id,
                ClientSecret = _clientConfig.Secret
            });

            _logger.LogInformation($"AUTHORITY: {_authConfig.Authority}----------------------------");

            _logger.LogInformation($"ACCESSTOKEN: {tokenResponse?.AccessToken} --- STATUSCODE{tokenResponse?.HttpResponse?.StatusCode}--------------------");
            client.SetBearerToken(tokenResponse?.AccessToken);

            var httpMessage = new HttpRequestMessage();
            httpMessage.RequestUri = new Uri(url);
            httpMessage.Method = method;

            if (content != null)
            {
                httpMessage.Content =
                    new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }

            var result = await client.SendAsync(httpMessage);

            _logger.LogInformation($"CONTENT: {await result?.Content?.ReadAsStringAsync()} -----------------------------");
            if (result.IsSuccessStatusCode)
            {
                var resultContent = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TResponse>(resultContent);
                return response!;
            }

            return default(TResponse)!;
        }
    }
}
