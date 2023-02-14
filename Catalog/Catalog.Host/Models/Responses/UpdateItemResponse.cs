namespace Catalog.Host.Models.Responses
{
    public class UpdateItemResponse<T>
    {
        public T IsUpdated { get; set; } = default(T) !;
    }
}
