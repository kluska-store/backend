using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Tests.Common.Builders;

namespace KluskaStore.Tests.Domain.Entities.Accounts;

public sealed class StoreSessionTests : SessionTests
{
    private readonly StoreSession _sut = StoreSessionBuilder.Valid();

    protected override Session GetSut() => _sut;

    [Fact]
    public void GivenValidInitialData_WhenCreatingNewStoreSession_ThenCreatesEntity()
    {
        var result = StoreSession.Create(_sut.Token, _sut.StoreId, _sut.CreatedAt);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Token.Should().Be(_sut.Token);
        result.Value.CreatedAt.Should().Be(_sut.CreatedAt);
        result.Value.StoreId.Should().Be(_sut.StoreId);
        result.Value.ExpiresAt.Should().Be(default);
    }

    [Fact]
    public void GivenInvalidCreationDate_WhenCreatingNewStoreSession_ThenReturnsFailure()
    {
        var result = StoreSession.Create(_sut.Token, _sut.StoreId, DateTime.UtcNow.AddDays(1));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.InvalidCreationDate.Code);
    }

    [Fact]
    public void GivenEmptyStoreId_WhenCreatingNewStoreSession_ThenReturnsFailure()
    {
        var result = StoreSession.Create(_sut.Token, Guid.Empty, _sut.CreatedAt);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.EmptyOwnerId.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenNoTokenIsProvided_ThenReturnsFailure()
    {
        var result = StoreSession.Create("", _sut.StoreId, _sut.CreatedAt);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.EmptySessionToken.Code);
    }
}
