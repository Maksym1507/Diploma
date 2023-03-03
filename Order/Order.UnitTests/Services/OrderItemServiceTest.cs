using Moq;
using Order.Host.Data.Entities;
using Order.Host.Models;
using Order.Host.Models.Dtos;

namespace Order.UnitTests.Services
{
    public class OrderItemServiceTest
    {
        private readonly OrderItemService _orderItemService;

        private readonly Mock<IOrderItemRepository> _orderItemRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<OrderItemService>> _logger;

        private readonly OrderEntity _testOrderSuccess = new()
        {
            Address = "Test"
        };

        private readonly OrderDto _testOrderDtoSuccess = new()
        {
            Address = "Test"
        };

        private readonly OrderDto _testOrder = new()
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

        public OrderItemServiceTest()
        {
            _orderItemRepository = new Mock<IOrderItemRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<OrderItemService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _orderItemService = new OrderItemService(_dbContextWrapper.Object, _logger.Object, _orderItemRepository.Object, _logger.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            var testResult = 1;

            _orderItemRepository.Setup(s => s.AddOrderAsync(
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
            var result = await _orderItemService.AddAsync(
                _testOrder.UserId,
                _testOrder.Name,
                _testOrder.LastName,
                _testBasketItemsList.ToArray(),
                _testOrder.PhoneNumber,
                _testOrder.Email,
                _testOrder.Country,
                _testOrder.Region,
                _testOrder.City,
                _testOrder.Address,
                _testOrder.Index,
                _testOrder.TotalSum);

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
        public async Task AddAsync_Failed()
        {
            // arrange
            var testResult = 0;

            _orderItemRepository.Setup(s => s.AddOrderAsync(
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
            var result = await _orderItemService.AddAsync(
                _testOrder.UserId,
                _testOrder.Name,
                _testOrder.LastName,
                _testBasketItemsList.ToArray(),
                _testOrder.PhoneNumber,
                _testOrder.Email,
                _testOrder.Country,
                _testOrder.Region,
                _testOrder.City,
                _testOrder.Address,
                _testOrder.Index,
                _testOrder.TotalSum);

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
        public async Task GetOrderByIdAsync_Success()
        {
            // arrange
            var testId = 1;

            _orderItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(_testOrderSuccess);

            _mapper.Setup(s => s.Map<OrderDto>(
            It.Is<OrderEntity>(i => i.Equals(_testOrderSuccess)))).Returns(_testOrderDtoSuccess);

            // act
            var result = await _orderItemService.GetOrderByIdAsync(testId);

            // assert
            result.Should().NotBeNull();
            result.Should().Be(_testOrderDtoSuccess);
        }

        [Fact]
        public async Task GetOrderByIdAsync_Failed()
        {
            // arrange
            var testId = 190;

            _orderItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync((Func<OrderEntity>)null!);

            // act
            var result = await _orderItemService.GetOrderByIdAsync(testId);

            // assert
            result.Should().BeNull();

            _logger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Not founded order item with Id = {testId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_Success()
        {
            // arrange
            int testId = 1;
            bool testResult = true;

            _orderItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(_testOrderSuccess);

            _orderItemRepository.Setup(s => s.DeleteOrderAsync(
                It.Is<OrderEntity>(i => i == _testOrderSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _orderItemService.DeleteOrderAsync(testId);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Removed order with Id = {testId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            int testId = 1;
            bool testResult = false;

            _orderItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync((Func<OrderEntity>)null!);

            _orderItemRepository.Setup(s => s.DeleteOrderAsync(
                It.Is<OrderEntity>(i => i == _testOrderSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _orderItemService.DeleteOrderAsync(testId);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Not founded order with Id = {testId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }
    }
}
