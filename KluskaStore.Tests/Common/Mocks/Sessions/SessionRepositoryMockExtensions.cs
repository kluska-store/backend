using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Domain.Entities.Accounts;

namespace KluskaStore.Tests.Common.Mocks.Sessions;

public static class SessionRepositoryMockExtensions
{
    public static void SetupUnregisterSessionByTokenAsyncReturnsCompletedTask(
        this Mock<ISessionRepository> mock,
        string token
    ) => mock
        .Setup(repo => repo.UnregisterAsync(token, It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    public static void SetupRegisterSessionReturns(this Mock<ISessionRepository> mock, string createdSessionToken) => mock
        .Setup(repo => repo.RegisterAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(createdSessionToken);

    public static void SetupGetSessionByTokenReturnsNull(this Mock<ISessionRepository> mock) => mock
        .Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Session?)null);

    public static void SetupGetSessionByTokenReturns(this Mock<ISessionRepository> mock, Session session) => mock
        .Setup(repo => repo.GetByTokenAsync(session.Token, It.IsAny<CancellationToken>()))
        .ReturnsAsync(session);

    public static void VerifyUnregisterSessionByTokenAsyncCalledOnce(this Mock<ISessionRepository> mock) => mock.Verify(
        repo => repo.UnregisterAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    public static void VerifyRegisterSessionCalled(this Mock<ISessionRepository> mock, Func<Times> times) => mock.Verify(
        repo => repo.RegisterAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
        times
    );

    public static void VerifyGetSessionByTokenCalled(this Mock<ISessionRepository> mock, Func<Times> times) => mock.Verify(
        repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        times
    );
}
