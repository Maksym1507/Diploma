namespace Catalog.Host.Models.Responses
{
    public class DeleteItemResponse<T>
    {
        public T IsDeleted { get; set; } = default(T) !;
    }
}
