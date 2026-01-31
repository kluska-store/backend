using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Tests.Common.Builders;

namespace KluskaStore.Tests.Domain.Entities.Accounts;

public sealed class UserSessionTests : SessionTests
{
    private readonly UserSession _sut = UserSessionBuilder.Valid();

    protected override Session GetSut() => _sut;

    [Fact]
    public void GivenValidInitialData_WhenCreatingNewUserSession_ThenCreatesEntity()
    {
        var result = UserSession.Create(_sut.Token, _sut.UserId, _sut.CreatedAt);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Token.Should().Be(_sut.Token);
        result.Value.CreatedAt.Should().Be(_sut.CreatedAt);
        result.Value.UserId.Should().Be(_sut.UserId);
        result.Value.ExpiresAt.Should().Be(default);
    }

    [Fact]
    public void GivenInvalidCreationDate_WhenCreatingNewUserSession_ThenReturnsFailure()
    {
        var result = UserSession.Create(_sut.Token, _sut.UserId, DateTime.UtcNow.AddDays(1));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.InvalidCreationDate.Code);
    }

    [Fact]
    public void GivenEmptyUserId_WhenCreatingNewUserSession_ThenReturnsFailure()
    {
        var result = UserSession.Create(_sut.Token, Guid.Empty, _sut.CreatedAt);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.EmptyOwnerId.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenNoTokenIsProvided_ThenReturnsFailure()
    {
        var result = UserSession.Create("", _sut.UserId, _sut.CreatedAt);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.EmptySessionToken.Code);
    }
}
