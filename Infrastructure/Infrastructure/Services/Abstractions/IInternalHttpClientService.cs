namespace Infrastructure.Services.Abstractions
{
    public interface IInternalHttpClientService
    {
        Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content);
    }
}
