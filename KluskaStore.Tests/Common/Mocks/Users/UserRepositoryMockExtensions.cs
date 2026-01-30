using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Tests.Common.Mocks.Users;

public static class UserRepositoryMockExtensions
{
    public static void VerifyGetUserByIdCalled(this Mock<IUserRepository> mock, Func<Times> times) => mock.Verify(
        repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void VerifyAddUserCalled(this Mock<IUserRepository> mock, Func<Times> times) => mock.Verify(
        repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void VerifyGetUserByEmailCalled(this Mock<IUserRepository> mock, Func<Times> times) => mock.Verify(
        repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void SetupGetUserByIdReturns(this Mock<IUserRepository> mock, User user) => mock
        .Setup(repo => repo.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(user);

    public static void SetupGetUserByIdReturnsNull(this Mock<IUserRepository> mock) => mock
        .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((User?)null);

    public static void SetupAddUserReturns(this Mock<IUserRepository> mock, Task returnValue) => mock
        .Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
        .Returns(returnValue);

    public static void SetupGetUserByEmailReturnsNull(this Mock<IUserRepository> mock) => mock
        .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((User?)null);

    public static void SetupGetUserByEmailReturns(this Mock<IUserRepository> mock, User user) => mock
        .Setup(repo => repo.GetByEmailAsync(user.Email.Value, It.IsAny<CancellationToken>()))
        .ReturnsAsync(user);
}
