using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Handlers.Query.GetApiKeyById;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Test.Handlers.Query.GetApiKeyById;

public class GetApiKeyById_Tests
{
    private readonly Mock<ILogger<GetApiKeyByIdQueryHandler>> _mockLogger;
    private readonly Mock<IRepository> _mockTenantRepository;

    public GetApiKeyById_Tests()
    {
        _mockTenantRepository = new Mock<IRepository>();
        _mockLogger = new Mock<ILogger<GetApiKeyByIdQueryHandler>>();
    }


    [Fact]
    public async Task Handler_Should_Return_ApiKey()
    {
        // Arrange
        var tenant1 = new Persistence.Models.Tenant {Id = 1, Name = "Tenant 1", TenantApiKey = "Key1"};
        var tenant2 = new Persistence.Models.Tenant {Id = 2, Name = "Tenant 2", TenantApiKey = "Key2"};
        var tenant3 = new Persistence.Models.Tenant {Id = 3, Name = "Tenant 3", TenantApiKey = "Key3"};
        var tenants = new List<Persistence.Models.Tenant> {tenant1, tenant2, tenant3};

        var query = new GetApiKeyByIdQuery(1);
        var handler = new GetApiKeyByIdQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetApiKeyById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something<string>("Key1"));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetApiKeyById(It.IsAny<int>(), default), Times.Once);
        result.Status.Should().Be(FluentResultsStatus.Success);
    }

    [Fact]
    public async Task Handler_Should_Return_Exception()
    {
        // Arrange
        var query = new GetApiKeyByIdQuery(1);
        var handler = new GetApiKeyByIdQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetApiKeyById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error."));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetApiKeyById(It.IsAny<int>(), default), Times.AtLeast(5));
        result.Status.Should().Be(FluentResultsStatus.Failure);
    }

    [Fact]
    public async Task Handler_Should_Return_Not_Found()
    {
        // Arrange
        var query = new GetApiKeyByIdQuery(1);
        var handler = new GetApiKeyByIdQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetApiKeyById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.NotFound<string>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetApiKeyById(It.IsAny<int>(), default), Times.Once);
        result.Status.Should().Be(FluentResultsStatus.NotFound);
    }
}
