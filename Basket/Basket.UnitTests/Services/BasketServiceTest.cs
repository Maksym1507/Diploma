namespace Basket.UnitTests.Services
{
    public class BasketServiceTest
    {
        private readonly IBasketService _basketService;

        private readonly Mock<ICacheService> _cacheService;
        private readonly Mock<ILogger<BasketService>> _logger;

        private readonly string _testUserId = "testUser";

        private readonly ProductToBasketModel _testProductToBasketModel = new()
        {
            Id = 1,
            PictureUrl = "test",
            Price = 1,
            Title = "Test",
        };

        private readonly BasketItemModel _testBasketItemModel = new()
        {
            Id = 1,
            Title = "Test",
            Price = 1,
            PictureUrl = "Test",
            Count = 1
        };

        public BasketServiceTest()
        {
            _cacheService = new Mock<ICacheService>();
            _logger = new Mock<ILogger<BasketService>>();

            _basketService = new BasketService(_cacheService.Object, _logger.Object);
        }

        [Fact]
        public async void GetBasketAsync__Success()
        {
            // act
            int testTotalSum = 1;

            var basketModelSuccess = new BasketModel()
            {
                BasketItems = new List<BasketItemModel>()
                {
                    _testBasketItemModel
                },
                TotalSum = testTotalSum,
            };

            _cacheService.Setup(s => s.GetAsync<BasketModel>(
                It.Is<string>(i => i == _testUserId))).ReturnsAsync(basketModelSuccess);

            // assert
            var result = await _basketService.GetBasketAsync(_testUserId);

            // assert
            result.Should().NotBeNull();
            result?.BasketItems.Should().NotBeNull();
            result?.TotalSum.Should().Be(testTotalSum);
        }

        [Fact]
        public async Task GetBasketAsync_ShouldReturnNewBasket_WhenBasketDoesNotExist()
        {
            // arrange
            _cacheService.Setup(x => x.GetAsync<BasketModel>(_testUserId)).ReturnsAsync((Func<BasketModel>)null!);

            // act
            var result = await _basketService.GetBasketAsync(_testUserId);

            // assert
            result.Should().NotBeNull();
            result.BasketItems.Count.Should().Be(0);
            result.TotalSum.Should().Be(0);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Not founded basket with user id = {_testUserId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task AddItemToBasketAsync_ShouldIncreaseBasketItemCount_WhenBasketItemExist()
        {
            // arrange
            int testTotalSum = 1;

            var basketModelSuccess = new BasketModel()
            {
                BasketItems = new List<BasketItemModel>()
                {
                    _testBasketItemModel
                },
                TotalSum = testTotalSum,
            };

            _cacheService.Setup(s => s.GetAsync<BasketModel>(
                It.Is<string>(i => i == _testUserId))).ReturnsAsync(basketModelSuccess);

            _cacheService.Setup(s => s.AddOrUpdateAsync(
                It.Is<string>(i => i == _testUserId),
                It.Is<BasketModel>(i => i == basketModelSuccess)));

            // act
            var result = await _basketService.AddItemToBasketAsync(_testUserId, _testProductToBasketModel);

            // assert
            result.Should().BeTrue();

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Item's count with id = {_testProductToBasketModel.Id} was updated")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task AddItemToBasketAsync_AddItem_WhenBasketItemNotExist()
        {
            // arrange
            int testTotalSum = 1;

            var testProductToBasketModel = new ProductToBasketModel()
            {
                Id = 2,
                PictureUrl = "test",
                Price = 1,
                Title = "Test",
            };

            var basketModelSuccess = new BasketModel()
            {
                BasketItems = new List<BasketItemModel>()
                {
                    _testBasketItemModel
                },
                TotalSum = testTotalSum,
            };

            _cacheService.Setup(s => s.GetAsync<BasketModel>(
                It.Is<string>(i => i == _testUserId))).ReturnsAsync(basketModelSuccess);

            _cacheService.Setup(s => s.AddOrUpdateAsync(
                It.Is<string>(i => i == _testUserId),
                It.Is<BasketModel>(i => i == basketModelSuccess)));

            // act
            var result = await _basketService.AddItemToBasketAsync(_testUserId, testProductToBasketModel);

            // assert
            result.Should().BeTrue();

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Item with id = {testProductToBasketModel.Id} was added to basket")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task AddItemToBasketAsync_AddItemBasket_IfBasketNotExist()
        {
            // arrange
            int testTotalSum = 1;

            var basketModelSuccess = new BasketModel()
            {
                BasketItems = new List<BasketItemModel>()
                {
                    _testBasketItemModel
                },
                TotalSum = testTotalSum,
            };

            _cacheService.Setup(x => x.GetAsync<BasketModel>(_testUserId)).ReturnsAsync((Func<BasketModel>)null!);

            _cacheService.Setup(s => s.AddOrUpdateAsync(
                It.Is<string>(i => i == _testUserId),
                It.Is<BasketModel>(i => i == basketModelSuccess)));

            // act
            var result = await _basketService.AddItemToBasketAsync(_testUserId, _testProductToBasketModel);

            // assert
            result.Should().BeTrue();

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Not founded basket with user id = {_testUserId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Item with id = {_testProductToBasketModel.Id} was added to basket")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DeleteBasketItemAsync_ShouldDicreaseBasketItemCount_WhenBasketItemExists()
        {
            // arrange
            int testTotalSum = 1;
            int testBasketItemId = 1;

            var basketModelSuccess = new BasketModel()
            {
                BasketItems = new List<BasketItemModel>()
                {
                    new BasketItemModel()
                    {
                        Id = 1,
                        Title = "Test",
                        Price = 1,
                        PictureUrl = "Test",
                        Count = 2
                    }
                },
                TotalSum = testTotalSum,
            };

            _cacheService.Setup(s => s.GetAsync<BasketModel>(
                It.Is<string>(i => i == _testUserId))).ReturnsAsync(basketModelSuccess);

            _cacheService.Setup(s => s.AddOrUpdateAsync(
                It.Is<string>(i => i == _testUserId),
                It.Is<BasketModel>(i => i == basketModelSuccess)));

            // act
            var result = await _basketService.DeleteBasketItemAsync(_testUserId, testBasketItemId);

            // assert
            result.Should().BeTrue();

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Item's count with id = {_testProductToBasketModel.Id} was updated")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DeleteBasketItemAsync_ShouldRemoveBasketItem_WhenBasketItemExists()
        {
            // arrange
            int testTotalSum = 1;
            int testBasketItemId = 1;

            var basketModelSuccess = new BasketModel()
            {
                BasketItems = new List<BasketItemModel>()
                {
                    _testBasketItemModel
                },
                TotalSum = testTotalSum,
            };

            _cacheService.Setup(s => s.GetAsync<BasketModel>(
                It.Is<string>(i => i == _testUserId))).ReturnsAsync(basketModelSuccess);

            _cacheService.Setup(s => s.AddOrUpdateAsync(
                It.Is<string>(i => i == _testUserId),
                It.Is<BasketModel>(i => i == basketModelSuccess)));

            // act
            var result = await _basketService.DeleteBasketItemAsync(_testUserId, testBasketItemId);

            // assert
            result.Should().BeTrue();

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Basket item with id = {testBasketItemId} was deleted")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DeleteBasketItemAsync_ReturnsFalse_IfBasketNotExist()
        {
            // arrange
            int testTotalSum = 1;
            int testBasketItemId = 1;

            var basketModelSuccess = new BasketModel()
            {
                BasketItems = new List<BasketItemModel>()
                {
                    _testBasketItemModel
                },
                TotalSum = testTotalSum,
            };

            _cacheService.Setup(s => s.GetAsync<BasketModel>(_testUserId)).ReturnsAsync((Func<BasketModel>)null!);

            _cacheService.Setup(s => s.AddOrUpdateAsync(
                It.Is<string>(i => i == _testUserId),
                It.Is<BasketModel>(i => i == basketModelSuccess)));

            // act
            var result = await _basketService.DeleteBasketItemAsync(_testUserId, testBasketItemId);

            // assert
            result.Should().BeFalse();

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Not founded basket with user id = {_testUserId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DeleteBasketItemAsync_ReturnsFalse_IfBasketItemNotExist()
        {
            // arrange
            int testTotalSum = 1;
            int testBasketItemId = 2;

            var basketModelSuccess = new BasketModel()
            {
                BasketItems = new List<BasketItemModel>()
                {
                    _testBasketItemModel
                },
                TotalSum = testTotalSum,
            };

            _cacheService.Setup(s => s.GetAsync<BasketModel>(_testUserId)).ReturnsAsync(basketModelSuccess);

            _cacheService.Setup(s => s.AddOrUpdateAsync(
                It.Is<string>(i => i == _testUserId),
                It.Is<BasketModel>(i => i == basketModelSuccess)));

            // act
            var result = await _basketService.DeleteBasketItemAsync(_testUserId, testBasketItemId);

            // assert
            result.Should().BeFalse();

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Basket item with id = {testBasketItemId} not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task TruncateBasketAsync_Success()
        {
            // arrange
            var testRemoveResponse = true;

            _cacheService.Setup(s => s.RemoveAsync(_testUserId)).ReturnsAsync(testRemoveResponse);

            // act
            var result = await _basketService.TruncateBasketAsync(_testUserId);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task TruncateBasketAsync_Failed()
        {
            // arrange
            var testRemoveResponse = false;

            _cacheService.Setup(s => s.RemoveAsync(_testUserId)).ReturnsAsync(testRemoveResponse);

            // act
            var result = await _basketService.TruncateBasketAsync(_testUserId);

            // assert
            result.Should().BeFalse();
        }
    }
}