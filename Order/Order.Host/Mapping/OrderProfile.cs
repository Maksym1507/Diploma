using Order.Host.Models.Requests;

namespace Order.Host.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddOrderRequest, OrderEntity>()
                .AfterMap((source, destination) => destination.CreatedAt = DateTime.UtcNow.Date);

            CreateMap<CartItemRequest, OrderDetailsEntity>();

            CreateMap<OrderEntity, OrderResponse>()
                .ForMember(destination => destination.OrderProducts, source => source.MapFrom(src => src.OrderDetails.ToArray()));

            CreateMap<OrderDetailsEntity, OrderDetailsResponse>();
        }
    }
}
