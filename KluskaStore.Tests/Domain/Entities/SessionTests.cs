using FluentAssertions;
using KluskaStore.Domain.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.Entities;

public class SessionTests
{
    [Fact]
    public void GivenValidUserSession_WhenSessionNotExpired_ThenCreatesEntity()
    {
        var result = Session.CreateUserSession(Guid.NewGuid(), DateTime.UtcNow);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Session>();
        result.Value.Owner.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.User);
        result.Value.IsExpired().Should().BeFalse();
    }

    [Fact]
    public void GivenValidUserSession_WhenSessionExpired_ThenCreatesEntity()
    {
        var result = Session.CreateUserSession(Guid.NewGuid(), DateTime.UtcNow.AddYears(-2));

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Session>();
        result.Value.Owner.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.User);
        result.Value.IsExpired().Should().BeTrue();
    }

    [Fact]
    public void GivenValidStoreSession_WhenSessionNotExpired_ThenCreatesEntity()
    {
        var result = Session.CreateStoreSession(Guid.NewGuid(), DateTime.UtcNow);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Session>();
        result.Value.Owner.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.Store);
        result.Value.IsExpired().Should().BeFalse();
    }

    [Fact]
    public void GivenValidStoreSession_WhenSessionExpired_ThenCreatesEntity()
    {
        var result = Session.CreateStoreSession(Guid.NewGuid(), DateTime.UtcNow.AddYears(-2));

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<Session>();
        result.Value.Owner.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.Store);
        result.Value.IsExpired().Should().BeTrue();
    }

    // All public factory method use the same base method to centralize validation
    [Fact]
    public void GivenInvalidSession_WhenCreatedAtFutureDate_ThenReturnsFailure()
    {
        var result = Session.CreateUserSession(Guid.NewGuid(), DateTime.UtcNow.AddDays(1));

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void GivenInvalidSession_WhenCreatedWithInvalidId_ThenReturnsFailure()
    {
        var result = Session.CreateUserSession(Guid.Empty, DateTime.UtcNow);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}
