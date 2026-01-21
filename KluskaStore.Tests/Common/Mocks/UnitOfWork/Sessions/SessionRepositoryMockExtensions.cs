using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Tests.Common.Mocks.UnitOfWork.Sessions;

public static class SessionRepositoryMockExtensions
{
    public static void SetupUnregisterSessionByTokenAsyncReturnsCompletedTask(
        this Mock<IUnitOfWork> mock,
        string token
    ) => mock
        .Setup(uow => uow.Sessions.UnregisterAsync(token, It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    public static void SetupRegisterSessionReturns(this Mock<IUnitOfWork> mock, string createdSessionToken) => mock
        .Setup(uow => uow.Sessions.RegisterAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(createdSessionToken);

    public static void SetupGetSessionByTokenReturnsNull(this Mock<IUnitOfWork> mock) => mock
        .Setup(uow => uow.Sessions.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Session?)null);

    public static void SetupGetSessionByTokenReturns(this Mock<IUnitOfWork> mock, Session session) => mock
        .Setup(uow => uow.Sessions.GetByTokenAsync(session.Token, It.IsAny<CancellationToken>()))
        .ReturnsAsync(session);

    public static void VerifyUnregisterSessionByTokenAsyncCalledOnce(this Mock<IUnitOfWork> mock) => mock.Verify(
        uow => uow.Sessions.UnregisterAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    public static void VerifyRegisterSessionCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.Sessions.RegisterAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void VerifyGetSessionByTokenCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.Sessions.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        times
    );
}
