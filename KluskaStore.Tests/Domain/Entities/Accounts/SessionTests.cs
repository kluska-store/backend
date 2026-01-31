using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Tests.Common.Builders;

namespace KluskaStore.Tests.Domain.Entities.Accounts;

public abstract class SessionTests
{
    protected abstract Session GetSut();

    [Fact]
    public void GivenExpirationDateDefinition_WhenExpirationDateIsValid_ThenSetsExpirationDate()
    {
        var sut = GetSut();
        var expirationDate = DateTime.UtcNow.AddDays(1);

        var result = sut.SetExpirationDate(expirationDate);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        var alteredSession = result.Value;
        alteredSession.Should().BeSameAs(sut);
        alteredSession.ExpiresAt.Should().Be(expirationDate);
        alteredSession.IsExpired().Should().BeFalse();
    }

    [Fact]
    public void GivenExpirationDateDefinition_WhenExpirationDateIsInvalid_ThenReturnsFailure()
    {
        var sut = GetSut();
        var expirationDate = DateTime.UtcNow;

        var result = sut.SetExpirationDate(expirationDate);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.InvalidExpirationDate.Code);
    }
}
