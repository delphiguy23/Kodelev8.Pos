using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Moq;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;
using Point.Of.Sale.Tenant.Handlers.Command.Patch;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Test.Handlers.Command.Patch;

public class PatchCommand_Test
{
    private readonly Mock<IRepository> _mockTenantRepository;
    private readonly Mock<ILogger<PatchCommandHandler>> _mockLogger;

    public PatchCommand_Test()
    {
        _mockTenantRepository = new Mock<IRepository>();
        _mockLogger = new Mock<ILogger<PatchCommandHandler>>();
    }

    [Fact]
    public async Task Handler_Should_Return_Success()
    {
        // Arrange
        var tenant1 = new Persistence.Models.Tenant {Id = 1, Name = "Tenant 1"};
        var tenant2 = new Persistence.Models.Tenant {Id = 2, Name = "Tenant 2"};
        var tenant3 = new Persistence.Models.Tenant {Id = 3, Name = "Tenant 3"};
        var tenants = new List<Persistence.Models.Tenant> {tenant1, tenant2, tenant3};

        var patch = new JsonPatchDocument<Persistence.Models.Tenant>();
        var command = new PatchCommand {Id = 1, Patch = patch};
        var handler = new PatchCommandHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.Patch(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something(new CrudResult<Persistence.Models.Tenant>
            {
                Count = 1,
                Entity = It.IsAny<Persistence.Models.Tenant>(),
            }));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.Patch(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), default), Times.Once);
        result.Status.Should().Be(FluentResultsStatus.Success);
    }

    [Fact]
    public async Task Handler_Should_Return_Failure()
    {
        // Arrange
        var tenant1 = new Persistence.Models.Tenant {Id = 1, Name = "Tenant 1"};
        var tenant2 = new Persistence.Models.Tenant {Id = 2, Name = "Tenant 2"};
        var tenant3 = new Persistence.Models.Tenant {Id = 3, Name = "Tenant 3"};
        var tenants = new List<Persistence.Models.Tenant> {tenant1, tenant2, tenant3};

        var patch = new JsonPatchDocument<Persistence.Models.Tenant>();
        var command = new PatchCommand {Id = 1, Patch = patch};
        var handler = new PatchCommandHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.Patch(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error."));


        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.Patch(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), default), Times.AtLeast(5));
        result.Status.Should().Be(FluentResultsStatus.Failure);
    }

    [Fact]
    public async Task Handler_Should_Return_NotFound()
    {
        // Arrange
        var tenant1 = new Persistence.Models.Tenant {Id = 1, Name = "Tenant 1"};
        var tenant2 = new Persistence.Models.Tenant {Id = 2, Name = "Tenant 2"};
        var tenant3 = new Persistence.Models.Tenant {Id = 3, Name = "Tenant 3"};
        var tenants = new List<Persistence.Models.Tenant> {tenant1, tenant2, tenant3};

        var patch = new JsonPatchDocument<Persistence.Models.Tenant>();
        var command = new PatchCommand {Id = 1, Patch = patch};
        var handler = new PatchCommandHandler(_mockLogger.Object, _mockTenantRepository.Object);
        _mockTenantRepository.Setup(x =>
                x.Patch(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.NotFound(new CrudResult<Persistence.Models.Tenant>
            {
                Count = 0,
                Entity = null,
            }));


        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.Patch(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), default), Times.Once);
        result.Status.Should().Be(FluentResultsStatus.NotFound);
    }
}
