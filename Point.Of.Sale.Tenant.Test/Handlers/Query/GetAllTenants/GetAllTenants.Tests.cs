using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Handlers.Query.GetAllTenants;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Test.Handlers.Query.GetAllTenants;

public class GetAllTenantsTests
{
    private readonly Mock<ILogger<GetAllQueryHandler>> _mockLogger;
    private readonly Mock<IRepository> _mockTenantRepository;

    public GetAllTenantsTests()
    {
        _mockTenantRepository = new Mock<IRepository>();
        _mockLogger = new Mock<ILogger<GetAllQueryHandler>>();
    }

    [Fact]
    public async Task Handler_Should_Return_All_Tenants()
    {
        // Arrange
        var tenant1 = new Persistence.Models.Tenant {Id = 1, Name = "Tenant 1"};
        var tenant2 = new Persistence.Models.Tenant {Id = 2, Name = "Tenant 2"};
        var tenant3 = new Persistence.Models.Tenant {Id = 3, Name = "Tenant 3"};
        var tenants = new List<Persistence.Models.Tenant> {tenant1, tenant2, tenant3};

        var query = new GetAll();
        var handler = new GetAllQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something<IEnumerable<Persistence.Models.Tenant>>(tenants));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Status.Should().Be(FluentResultsStatus.Success);
    }

    [Fact]
    public async Task Handler_Should_Return_Exception()
    {
        // Arrange
        var query = new GetAll();
        var handler = new GetAllQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetAll(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error."));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Status.Should().Be(FluentResultsStatus.Failure);
    }

    [Fact]
    public async Task Handler_Should_Return_Not_Found()
    {
        // Arrange
        var query = new GetAll();
        var handler = new GetAllQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.NotFound<IEnumerable<Persistence.Models.Tenant>>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Status.Should().Be(FluentResultsStatus.NotFound);
    }

    [Fact]
    public async Task Handler_Should_Return_Bad_Request()
    {
        // Arrange
        var query = new GetAll();
        var handler = new GetAllQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.BadRequest<IEnumerable<Persistence.Models.Tenant>>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Status.Should().Be(FluentResultsStatus.BadRequest);
    }

    [Fact]
    public async Task Handler_Should_Return_Failure()
    {
        // Arrange
        var query = new GetAll();
        var handler = new GetAllQueryHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Failure<IEnumerable<Persistence.Models.Tenant>>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Status.Should().Be(FluentResultsStatus.Failure);
    }
}
