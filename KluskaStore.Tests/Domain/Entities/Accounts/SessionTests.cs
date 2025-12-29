using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.Entities.Accounts;

public class SessionTests
{
    private static SessionOwner GenerateValidSessionOwner() => new(SessionOwner.OwnerTypeEnum.User, Guid.NewGuid());

    [Fact]
    public void GivenValidUserSession_WhenSessionNotExpired_ThenCreatesEntity()
    {
        var result = Session.Create(GenerateValidSessionOwner(), DateTime.UtcNow);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Owner.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.User);
        result.Value.IsExpired().Should().BeFalse();
    }

    [Fact]
    public void GivenValidUserSession_WhenSessionExpired_ThenCreatesEntity()
    {
        var result = Session.Create(GenerateValidSessionOwner(), DateTime.UtcNow.AddYears(-2));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Owner.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.User);
        result.Value.IsExpired().Should().BeTrue();
    }

    [Fact]
    public void GivenInvalidSession_WhenCreatedAtFutureDate_ThenReturnsFailure()
    {
        var result = Session.Create(GenerateValidSessionOwner(), DateTime.UtcNow.AddDays(1));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionErrors.InvalidCreationDate.Code);
    }
}
