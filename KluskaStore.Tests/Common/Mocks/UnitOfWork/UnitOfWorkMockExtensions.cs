using KluskaStore.Application.Abstractions.Persistence;

namespace KluskaStore.Tests.Common.Mocks.UnitOfWork;

public static class UnitOfWorkMockExtensions
{
    public static void VerifyCommitAsyncCalled(this Mock<IUnitOfWork> mock, Func<Times> times) => mock.Verify(
        uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
        times
    );
}
