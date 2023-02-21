using StackExchange.Redis;

namespace Basket.Host.Services.Abstractions
{
    public interface IRedisCacheConnectionService
    {
        public IConnectionMultiplexer Connection { get; }
    }
}
