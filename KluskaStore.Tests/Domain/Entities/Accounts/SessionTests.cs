using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Domain.ValueObjects;
using KluskaStore.Tests.Common.Builders;

namespace KluskaStore.Tests.Domain.Entities.Accounts;

public class SessionTests
{
    private readonly Session _sut = SessionBuilder.Valid(isUserSession: true);

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesEntity()
    {
        var result = Session.Create(_sut.Token, _sut.Owner, _sut.CreatedAt);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Owner.OwnerType.Should().Be(_sut.Owner.OwnerType);
        result.Value.Token.Should().Be(_sut.Token);
        result.Value.CreatedAt.Should().Be(_sut.CreatedAt);
        result.Value.ExpiresAt.Should().Be(default);
    }

    [Fact]
    public void GivenInvalidSession_WhenCreatedAtFutureDate_ThenReturnsFailure()
    {
        var result = Session.Create(_sut.Token, _sut.Owner, DateTime.UtcNow.AddDays(1));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.InvalidCreationDate.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenNoTokenIsProvided_ThenReturnsFailure()
    {
        var result = Session.Create("", _sut.Owner, _sut.CreatedAt);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.EmptySessionToken.Code);
    }

    [Fact]
    public void GivenExpirationDateDefinition_WhenExpirationDateIsValid_ThenSetsExpirationDate()
    {
        var expirationDate = DateTime.UtcNow.AddDays(1);

        var result = _sut.SetExpirationDate(expirationDate);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        var alteredSession = result.Value;
        alteredSession.Should().BeSameAs(_sut);
        alteredSession.ExpiresAt.Should().Be(expirationDate);
        alteredSession.IsExpired().Should().BeFalse();
    }

    [Fact]
    public void GivenExpirationDateDefinition_WhenExpirationDateIsInvalid_ThenReturnsFailure()
    {
        var expirationDate = DateTime.UtcNow;

        var result = _sut.SetExpirationDate(expirationDate);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.InvalidExpirationDate.Code);
    }
}
