namespace Catalog.Host.Models.Responses
{
    public class ItemsResponse<T>
    {
        public IEnumerable<T> Data { get; init; } = null!;
    }
}
