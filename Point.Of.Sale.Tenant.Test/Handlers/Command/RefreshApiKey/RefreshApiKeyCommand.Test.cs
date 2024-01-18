using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;
using Point.Of.Sale.Tenant.Handlers.Command.RefreshApiKey;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Tenant.Test.Handlers.Command.RefreshApiKey;

public class RefreshApiKeyCommand_Test
{
    private readonly Mock<IOptions<PosConfiguration>> _configurtation;
    private readonly Mock<ILogger<RefreshApiKeyCommandHandler>> _mockLogger;
    private readonly Mock<IRepository> _mockTenantRepository;
    private readonly List<Persistence.Models.Tenant> tenants;

    public RefreshApiKeyCommand_Test()
    {
        _mockTenantRepository = new Mock<IRepository>();
        _mockLogger = new Mock<ILogger<RefreshApiKeyCommandHandler>>();
        _configurtation = new Mock<IOptions<PosConfiguration>>();

        var tenant1 = new Persistence.Models.Tenant {Id = 1, Name = "Tenant 1", TenantApiKey = "api_key_1"};
        var tenant2 = new Persistence.Models.Tenant {Id = 2, Name = "Tenant 2", TenantApiKey = "api_key_2"};
        var tenant3 = new Persistence.Models.Tenant {Id = 3, Name = "Tenant 3", TenantApiKey = "api_key_3"};
        tenants = new List<Persistence.Models.Tenant> {tenant1, tenant2, tenant3};

        _configurtation.Setup(c => c.Value)
            .Returns
            (
                new PosConfiguration
                {
                    General = new General {ServiceName = "Test", SecretKey = "ThisIsASecretKey"},
                }
            );
    }

    [Theory]
    [InlineData(1, "api_key_1")]
    [InlineData(2, "api_key_2")]
    [InlineData(3, "api_key_3")]
    public async Task Handler_Should_Return_Api_Key(int id, string apiKey)
    {
        // Arrange
        var command = new RefreshApiKeyCommand(id, apiKey);
        var handler = new RefreshApiKeyCommandHandler(_mockLogger.Object, _configurtation.Object, _mockTenantRepository.Object);

        _mockTenantRepository.Setup(x =>
                x.GetById(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something(tenants.First(x => x.Id == id)));

        _mockTenantRepository.Setup(x =>
                x.Patch(id, It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something(new CrudResult<Persistence.Models.Tenant>
            {
                Count = 1,
                Entity = It.IsAny<Persistence.Models.Tenant>(),
            }));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(id, default), Times.Once);
        _mockTenantRepository.Verify(v => v.Patch(id, It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), default), Times.Once);
        result.Status.Should().Be(FluentResultsStatus.Success);
        result.Value.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData(4, "api_key_4")]
    [InlineData(5, "api_key_5")]
    [InlineData(6, "api_key_6")]
    public async Task Handler_Should_Return_NotFound(int id, string apikey)
    {
        // Arrange
        var command = new RefreshApiKeyCommand(id, apikey);
        var handler = new RefreshApiKeyCommandHandler(_mockLogger.Object, _configurtation.Object, _mockTenantRepository.Object);

        _mockTenantRepository.Setup(x =>
                x.GetById(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultsTo.NotFound<Persistence.Models.Tenant>);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(id, default), Times.Once);
        _mockTenantRepository.Verify(v => v.Patch(id, It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), default), Times.Never);
        result.Status.Should().Be(FluentResultsStatus.NotFound);
    }

    [Theory]
    [InlineData(1, "api_key_A")]
    [InlineData(2, "api_key_B")]
    [InlineData(3, "api_key_C")]
    public async Task Handler_Should_Return_BadRequestA(int id, string apiKey)
    {
        // Arrange
        var command = new RefreshApiKeyCommand(id, apiKey);
        var handler = new RefreshApiKeyCommandHandler(_mockLogger.Object, _configurtation.Object, _mockTenantRepository.Object);

        _mockTenantRepository.Setup(x =>
                x.GetById(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something(tenants.First(x => x.Id == id)));

        _mockTenantRepository.Setup(x =>
                x.Patch(id, It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something(new CrudResult<Persistence.Models.Tenant>
            {
                Count = 1,
                Entity = It.IsAny<Persistence.Models.Tenant>(),
            }));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(id, default), Times.Once);
        _mockTenantRepository.Verify(v => v.Patch(id, It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), default), Times.Never);
        result.Status.Should().Be(FluentResultsStatus.BadRequest);
        result.Messages.Should().Contain("Api Key mismatched");
    }

    [Theory]
    [InlineData(1, "api_key_1")]
    [InlineData(2, "api_key_2")]
    [InlineData(3, "api_key_3")]
    public async Task Handler_Should_Return_Failure(int id, string apiKey)
    {
        // Arrange
        var command = new RefreshApiKeyCommand(id, apiKey);
        var handler = new RefreshApiKeyCommandHandler(_mockLogger.Object, _configurtation.Object, _mockTenantRepository.Object);

        _configurtation.Setup(c => c.Value)
            .Returns
            (
                new PosConfiguration
                {
                    General = new General {ServiceName = "Test", SecretKey = "_"},
                }
            );

        _mockTenantRepository.Setup(x =>
                x.GetById(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something(tenants.First(x => x.Id == id)));

        _mockTenantRepository.Setup(x =>
                x.Patch(id, It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ResultsTo.Something(new CrudResult<Persistence.Models.Tenant>
            {
                Count = 1,
                Entity = It.IsAny<Persistence.Models.Tenant>(),
            }));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(id, default), Times.Once);
        _mockTenantRepository.Verify(v => v.Patch(id, It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), default), Times.Never);
        result.Status.Should().Be(FluentResultsStatus.Failure);
        result.Messages.Should().Contain("Failed to generate api key");
    }

    [Fact]
    public async Task Handler_Should_Return_FailureB()
    {
        // Arrange
        var command = new RefreshApiKeyCommand(1, "api_key_1");
        var handler = new RefreshApiKeyCommandHandler(_mockLogger.Object, _configurtation.Object, _mockTenantRepository.Object);

        _mockTenantRepository.Setup(x =>
                x.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockTenantRepository.Verify(v => v.GetById(It.IsAny<int>(), default), Times.AtLeast(5));
        _mockTenantRepository.Verify(v => v.Patch(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Persistence.Models.Tenant>>(), default), Times.Never);
        result.Status.Should().Be(FluentResultsStatus.NotFound);
    }
}
