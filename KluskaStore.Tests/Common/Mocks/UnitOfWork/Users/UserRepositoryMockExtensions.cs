using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Tests.Common.Mocks.UnitOfWork.Users;

public static class UserRepositoryMockExtensions
{
    public static void VerifyGetUserByIdCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.Users.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void VerifyAddUserCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.Users.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void VerifyGetUserByEmailCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void SetupGetUserByIdReturns(this Mock<IUnitOfWork> mock, User user) => mock
        .Setup(uow => uow.Users.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(user);

    public static void SetupGetUserByIdReturnsNull(this Mock<IUnitOfWork> mock) => mock
        .Setup(uow => uow.Users.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((User?)null);

    public static void SetupAddUserReturns(this Mock<IUnitOfWork> mock, Guid addedUserId) => mock
        .Setup(uow => uow.Users.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(addedUserId);

    public static void SetupGetUserByEmailReturnsNull(this Mock<IUnitOfWork> mock) => mock
        .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((User?)null);

    public static void SetupGetUserByEmailReturns(this Mock<IUnitOfWork> mock, User user) => mock
        .Setup(uow => uow.Users.GetByEmailAsync(user.Email.Value, It.IsAny<CancellationToken>()))
        .ReturnsAsync(user);
}
