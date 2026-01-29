using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Users.GetUserById;
using KluskaStore.Tests.Common.Builders;
using KluskaStore.Tests.Common.Mocks.Users;

namespace KluskaStore.Tests.Application.Features.Users.GetUserById;

public class GetUserByIdTests
{
    private readonly Mock<IUserRepository> _mock = new();
    private readonly GetUserByIdHandler _sut;

    public GetUserByIdTests() => _sut = new GetUserByIdHandler(_mock.Object);


    [Fact]
    public async Task GivenExistingUser_WhenGettingById_ThenReturnsUserDto()
    {
        var user = UserBuilder.Valid();
        _mock.SetupGetUserByIdReturns(user);

        var result = await _sut.Handle(new GetUserByIdQuery(user.Id));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        _mock.VerifyGetUserByIdCalled(Times.Once);
    }

    [Fact]
    public async Task GivenNonExistingUser_WhenGettingById_ThenReturnsNull()
    {
        _mock.SetupGetUserByIdReturnsNull();

        var result = await _sut.Handle(new GetUserByIdQuery(Guid.NewGuid()));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(GetUserByIdErrors.NotFound.Code);
        _mock.VerifyGetUserByIdCalled(Times.Once);
    }
}
