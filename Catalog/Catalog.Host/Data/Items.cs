namespace Catalog.Host.Data
{
    public class Items<T>
    {
        public IEnumerable<T> Data { get; init; }
    }
}
