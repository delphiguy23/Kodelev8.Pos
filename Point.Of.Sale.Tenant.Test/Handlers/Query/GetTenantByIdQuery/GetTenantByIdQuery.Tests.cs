using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Test.Handlers.Query.GetTenantByIdQuery;

public class GetTenantByIdQuery_Tests
{
    private readonly Mock<ILogger<GetTenantByIdQueryHandler>> _mockLogger;
    private readonly Mock<IRepository> _mockTenantRepository;

    public GetTenantByIdQuery_Tests()
    {
        _mockTenantRepository = new Mock<IRepository>();
        _mockLogger = new Mock<ILogger<GetTenantByIdQueryHandler>>();
    }

    [Fact]
    public async Task Handler_Should_Return_Tenant()
    {
        // Arrange
        var tenant1 = new Persistence.Models.Tenant {Id = 1, Name = "Tenant 1"};
        var tenant2 = new Persistence.Models.Tenant {Id = 2, Name = "Tenant 2"};
        var tenant3 = new Persistence.Models.Tenant {Id = 3, Name = "Tenant 3"};
        var tenants = new List<Persistence.Models.Tenant> {tenant1, tenant2, tenant3};

        var query = new GetTenantById(1);
        var handler = new GetTenantByIdQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something(tenant1));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(It.IsAny<int>(), default), Times.Once);
        result.Status.Should().Be(FluentResultsStatus.Success);
    }


    [Fact]
    public async Task Handler_Should_Return_Exception()
    {
        // Arrange
        var query = new GetTenantById(1);
        var handler = new GetTenantByIdQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error."));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(It.IsAny<int>(), default), Times.AtLeast(5));
        result.Status.Should().Be(FluentResultsStatus.Failure);
    }


    [Fact]
    public async Task Handler_Should_Return_Not_Found()
    {
        // Arrange
        var query = new GetTenantById(1);
        var handler = new GetTenantByIdQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.NotFound<Persistence.Models.Tenant>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(It.IsAny<int>(), default), Times.Once);
        result.Status.Should().Be(FluentResultsStatus.NotFound);
    }

    [Fact]
    public async Task Handler_Should_Return_Bad_Request()
    {
        // Arrange
        var query = new GetTenantById(1);
        var handler = new GetTenantByIdQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.BadRequest<Persistence.Models.Tenant>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(It.IsAny<int>(), default), Times.Once);
        result.Status.Should().Be(FluentResultsStatus.BadRequest);
    }

    [Fact]
    public async Task Handler_Should_Return_Failure()
    {
        // Arrange
        var query = new GetTenantById(1);
        var handler = new GetTenantByIdQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Failure<Persistence.Models.Tenant>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(It.IsAny<int>(), default), Times.Once);
        result.Status.Should().Be(FluentResultsStatus.Failure);
    }
}
