using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Users.GetUserById;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.ValueObjects.AccountData;
using KluskaStore.Tests.Common.Builders;

namespace KluskaStore.Tests.Application.Features.Users.GetUserById;

public class GetUserByIdTests
{
    private readonly Mock<IUserRepository> _mock = new();
    private readonly GetUserByIdHandler _sut;

    public GetUserByIdTests() => _sut = new GetUserByIdHandler(_mock.Object);

    private void VerifyGetUserByIdCalledOnce() => _mock.Verify(
        repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    [Fact]
    public async Task GivenExistingUser_WhenGettingById_ThenReturnsUserDto()
    {
        var user = UserBuilder.Valid();

        _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var result = await _sut.Handle(new GetUserByIdQuery(Guid.NewGuid()));

        result.Should().NotBeNull();
        VerifyGetUserByIdCalledOnce();
    }

    [Fact]
    public async Task GivenNonExistingUser_WhenGettingById_ThenReturnsNull()
    {
        _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var result = await _sut.Handle(new GetUserByIdQuery(Guid.NewGuid()));

        result.Should().BeNull();
        VerifyGetUserByIdCalledOnce();
    }
}
