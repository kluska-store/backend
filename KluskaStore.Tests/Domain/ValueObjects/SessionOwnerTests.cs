using KluskaStore.Domain.Errors.ValueObjects;
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
        result.Value.Should().NotBeNull();
        result.Value.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.User);
        result.Value.OwnerId.Should().Be(id);
    }

    [Fact]
    public void GivenValidSessionOwner_WhenOwnerIsStore_ThenCreatesVo()
    {
        var id = Guid.NewGuid();
        var result = SessionOwner.Store(id);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.OwnerType.Should().Be(SessionOwner.OwnerTypeEnum.Store);
        result.Value.OwnerId.Should().Be(id);
    }

    // Every public factory method for SessionOwner uses the same base method, so the verification is the same
    [Fact]
    public void GivenVoCreation_WhenOwnerIdIsEmpty_ThenReturnsFailure()
    {
        var result = SessionOwner.User(Guid.Empty);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(SessionOwnerErrors.EmptyOwnerId.Code);
    }
}
