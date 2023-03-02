namespace Order.Host.Models.Responses
{
    public class DeleteOrderItemResponse<T>
    {
        public T IsDeleted { get; set; } = default(T)!;
    }
}
