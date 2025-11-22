using FluentAssertions;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.ValueObjects;

public class SessionOwnerTests
{
    [Fact]
    public void GivenValidSessionOwner_WhenOwnerIsUser_ThenCreatesVo()
    {
        var id = Guid.NewGuid();
        var result = SessionOwner.User(id);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<SessionOwner>();
        result.Value.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.User);
        result.Value.OwnerId.Should().Be(id);
    }

    [Fact]
    public void GivenValidSessionOwner_WhenOwnerIsStore_ThenCreatesVo()
    {
        var id = Guid.NewGuid();
        var result = SessionOwner.Store(id);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<SessionOwner>();
        result.Value.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.Store);
        result.Value.OwnerId.Should().Be(id);
    }

    [Fact]
    public void GivenInvalidSessionOwner_ThenReturnsFailure()
    {
        // Every public factory method for SessionOwner uses the same base method, so the verification is the same
        var result = SessionOwner.User(Guid.Empty);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}
