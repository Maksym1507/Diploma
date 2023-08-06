using Order.Host.Configurations;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;

namespace Order.UnitTests.Services
{
    public class OrderServiceTest
    {
        private readonly IOrderService _orderService;

        private readonly Mock<IOrderRepository> _orderRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<OrderService>> _logger;
        private readonly Mock<IInternalHttpClientService> _httpClient;
        private readonly Mock<IOptions<OrderConfig>> _config;

        private readonly OrderDto _testOrder = new ()
        {
            UserId = "test",
            Name = "Test",
            LastName = "Test",
            PhoneNumber = "Test",
            Email = "Test",
            Country = "Test",
            Region = "Test",
            City = "Test",
            Address = "Test",
            Index = "Test",
            TotalSum = 1,
        };

        private readonly List<BasketItemModel> _testBasketItemsList = new()
        {
            new BasketItemModel()
            {
                Id = 1,
                Title = "Test",
                Price = 1,
                PictureUrl = "Test",
                Count = 1,
            }
        };

        public OrderServiceTest()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<OrderService>>();
            _httpClient = new Mock<IInternalHttpClientService>();
            _config = new Mock<IOptions<OrderConfig>>();

            _config.Setup(s => s.Value).Returns(new OrderConfig() { BasketUrl = "http://www.alevelwebsite.com:5003/api/v1/basketbff" });

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _orderService = new OrderService(_dbContextWrapper.Object, _logger.Object, _orderRepository.Object, _logger.Object, _mapper.Object, _httpClient.Object, _config.Object);
        }

        [Fact]
        public async Task DoOrderAsync_Success()
        {
            // arrange
            var testbasketModel = new BasketModel()
            {
                BasketItems = _testBasketItemsList,
                TotalSum = 1
            };

            var testResult = 1;

            _httpClient.Setup(s => s.SendAsync<BasketModel, GetBasketRequest>(
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                It.IsAny<GetBasketRequest>())).ReturnsAsync(testbasketModel);

            _orderRepository.Setup(s => s.AddOrderAsync(
                _testOrder.UserId,
                _testOrder.Name,
                _testOrder.LastName,
                _testOrder.PhoneNumber,
                _testOrder.Email,
                _testOrder.Country,
                _testOrder.Region,
                _testOrder.City,
                _testOrder.Address,
                _testOrder.Index,
                _testOrder.TotalSum,
                _testBasketItemsList)).ReturnsAsync(testResult);

            // act
            var result = await _orderService.DoOrderAsync(
                _testOrder.UserId,
                _testOrder.Name,
                _testOrder.LastName,
                _testOrder.PhoneNumber,
                _testOrder.Email,
                _testOrder.Country,
                _testOrder.Region,
                _testOrder.City,
                _testOrder.Address,
                _testOrder.Index);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Order with id = {testResult} has been added")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DoOrderAsync_ShouldReturnZero_WhenBasketIsEmpty()
        {
            // arrange
            var testResult = 0;

            _httpClient.Setup(s => s.SendAsync<BasketModel, GetBasketRequest>(
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                It.IsAny<GetBasketRequest>())).ReturnsAsync(new BasketModel());

            // act
            var result = await _orderService.DoOrderAsync(
                _testOrder.UserId,
                _testOrder.Name,
                _testOrder.LastName,
                _testOrder.PhoneNumber,
                _testOrder.Email,
                _testOrder.Country,
                _testOrder.Region,
                _testOrder.City,
                _testOrder.Address,
                _testOrder.Index);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Basket with id = {_testOrder.UserId} is empty")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DoOrderAsync_Failed()
        {
            // arrange
            var testbasketModel = new BasketModel()
            {
                BasketItems = _testBasketItemsList,
                TotalSum = 1
            };

            var testResult = 0;

            _httpClient.Setup(s => s.SendAsync<BasketModel, GetBasketRequest>(
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                It.IsAny<GetBasketRequest>())).ReturnsAsync(testbasketModel);

            _orderRepository.Setup(s => s.AddOrderAsync(
                _testOrder.UserId,
                _testOrder.Name,
                _testOrder.LastName,
                _testOrder.PhoneNumber,
                _testOrder.Email,
                _testOrder.Country,
                _testOrder.Region,
                _testOrder.City,
                _testOrder.Address,
                _testOrder.Index,
                _testOrder.TotalSum,
                _testBasketItemsList)).ReturnsAsync(testResult);

            // act
            var result = await _orderService.DoOrderAsync(
                _testOrder.UserId,
                _testOrder.Name,
                _testOrder.LastName,
                _testOrder.PhoneNumber,
                _testOrder.Email,
                _testOrder.Country,
                _testOrder.Region,
                _testOrder.City,
                _testOrder.Address,
                _testOrder.Index);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains("Failed add order to db")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task GetOrdersByUserIdAsync_Success()
        {
            //arrange
            var testUserId = "Test";
            var testOrderEntity = new List<OrderEntity>()
            {
                new OrderEntity
                {
                    Id = 1,
                    TotalSum = 1,
                    City = "Test"
                }
            };

            var testOrderResponse = new List<OrderResponse>()
            {
                new OrderResponse
                {
                    Id = 1,
                    TotalSum = 1,
                    City = "Test",
                }
            };

            _orderRepository.Setup(s => s.GetOrdersByUserIdAsync(testUserId))
                .ReturnsAsync(testOrderEntity);

            _mapper.Setup(s => s.Map<List<OrderResponse>>(
                It.Is<List<OrderEntity>>(i => i.Equals(testOrderEntity)))).Returns(testOrderResponse);

            // act
            var result = await _orderService.GetOrdersByUserIdAsync(testUserId);

            // act
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetOrdersByUserIdAsync_Failed()
        {
            // arrange
            var testUserId = "Test";
            var testOrderEntity = new List<OrderEntity>();

            _orderRepository.Setup(s => s.GetOrdersByUserIdAsync(testUserId))
                .ReturnsAsync(testOrderEntity);

            //act
            var result = await _orderService.GetOrdersByUserIdAsync(testUserId);

            // assert
            result.Should().BeNull();

            _logger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                    .Contains($"Not founded orders with user id = {testUserId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }
    }
}