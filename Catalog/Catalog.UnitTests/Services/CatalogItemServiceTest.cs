namespace Catalog.UnitTests.Services
{
    public class CatalogItemServiceTest
    {
        private readonly ICatalogItemService _catalogItemService;

        private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogItemService>> _logger;

        private readonly CatalogItem _testItemForAdd = new ()
        {
            Title = "TestTitle",
            Description = "Test",
            PictureFileName = "Test",
            Price = 10,
            Weight = 10,
            CatalogTypeId = 1,
            AvailableStock = 10,
        };

        private readonly CatalogItem _testItemForUpdateDeleteSuccess = new ()
        {
            Id = 1,
            Title = "TestTitle",
            Description = "Test",
            PictureFileName = "Test",
            Price = 10,
            Weight = 10,
            CatalogTypeId = 1,
            AvailableStock = 10,
        };

        private readonly CatalogItem _testItemForUpdate = new ()
        {
            Title = "TestTitle1",
            Description = "Test1",
            PictureFileName = "Test1",
            Price = 100,
            Weight = 100,
            CatalogTypeId = 10,
            AvailableStock = 100,
        };

        public CatalogItemServiceTest()
        {
            _catalogItemRepository = new Mock<ICatalogItemRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogItemService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogItemService = new CatalogItemService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            var testResult = 1;

            _catalogItemRepository.Setup(s => s.AddAsync(
                It.Is<string>(i => i == _testItemForAdd.Title),
                It.Is<string>(i => i == _testItemForAdd.Description),
                It.Is<decimal>(i => i == _testItemForAdd.Price),
                It.Is<double>(i => i == _testItemForAdd.Weight),
                It.Is<int>(i => i == _testItemForAdd.AvailableStock),
                It.Is<int>(i => i == _testItemForAdd.CatalogTypeId),
                It.Is<string>(i => i == _testItemForAdd.PictureFileName))).ReturnsAsync(testResult);

            // act
            var result = await _catalogItemService.AddAsync(_testItemForAdd.Title, _testItemForAdd.Description, _testItemForAdd.Price, _testItemForAdd.Weight, _testItemForAdd.AvailableStock, _testItemForAdd.CatalogTypeId, _testItemForAdd.PictureFileName);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Created catalog item with Id = {result.Value}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogItemRepository.Setup(s => s.AddAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<double>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogItemService.AddAsync(_testItemForAdd.Title, _testItemForAdd.Description, _testItemForAdd.Price, _testItemForAdd.Weight, _testItemForAdd.AvailableStock, _testItemForAdd.CatalogTypeId, _testItemForAdd.PictureFileName);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            // arrange
            int testId = 1;
            bool testResult = true;

            _catalogItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(_testItemForUpdateDeleteSuccess);

            _testItemForUpdateDeleteSuccess.Title = _testItemForUpdate.Title;
            _testItemForUpdateDeleteSuccess.Description = _testItemForUpdate.Description;
            _testItemForUpdateDeleteSuccess.Price = _testItemForUpdate.Price;
            _testItemForUpdateDeleteSuccess.Weight = _testItemForUpdate.Weight;
            _testItemForUpdateDeleteSuccess.AvailableStock = _testItemForUpdate.AvailableStock;
            _testItemForUpdateDeleteSuccess.CatalogTypeId = _testItemForUpdate.CatalogTypeId;
            _testItemForUpdateDeleteSuccess.PictureFileName = _testItemForUpdate.PictureFileName;

            _catalogItemRepository.Setup(s => s.UpdateAsync(
                It.Is<CatalogItem>(i => i == _testItemForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogItemService.UpdateAsync(testId, _testItemForUpdateDeleteSuccess.Title, _testItemForUpdateDeleteSuccess.Description, _testItemForUpdateDeleteSuccess.Price, _testItemForAdd.Weight, _testItemForUpdateDeleteSuccess.AvailableStock, _testItemForUpdateDeleteSuccess.CatalogTypeId, _testItemForUpdateDeleteSuccess.PictureFileName);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Updated catalog item with Id = {testId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            int testId = 1;
            bool testResult = false;

            _catalogItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync((Func<CatalogItem>)null!);

            _catalogItemRepository.Setup(s => s.UpdateAsync(
                It.Is<CatalogItem>(i => i == _testItemForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogItemService.UpdateAsync(testId, _testItemForUpdateDeleteSuccess.Title, _testItemForUpdateDeleteSuccess.Description, _testItemForUpdateDeleteSuccess.Price, _testItemForAdd.Weight, _testItemForUpdateDeleteSuccess.AvailableStock, _testItemForUpdateDeleteSuccess.CatalogTypeId, _testItemForUpdateDeleteSuccess.PictureFileName);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Not founded catalog item with Id = {testId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            int testId = 1;
            bool testResult = true;

            _catalogItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(_testItemForUpdateDeleteSuccess);

            _catalogItemRepository.Setup(s => s.DeleteAsync(
                It.Is<CatalogItem>(i => i == _testItemForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogItemService.DeleteAsync(testId);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Removed catalog item with Id = {testId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            int testId = 190;
            bool testResult = false;

            _catalogItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync((Func<CatalogItem>)null!);

            _catalogItemRepository.Setup(s => s.DeleteAsync(
                It.Is<CatalogItem>(i => i == _testItemForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogItemService.DeleteAsync(testId);

            // assert
            result.Should().Be(testResult);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                        .Contains($"Not founded catalog item with Id = {testId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }
    }
}