using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Sessions.EndSession;
using KluskaStore.Tests.Common.Mocks.UnitOfWork;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Sessions;

namespace KluskaStore.Tests.Application.Features.Sessions.EndSession;

public class EndSessionTests
{
    private readonly Mock<IUnitOfWork> _mock = new();
    private readonly EndSessionHandler _sut;

    public EndSessionTests() => _sut = new EndSessionHandler(_mock.Object);

    [Fact]
    public async Task GivenSessionEnd_ThenEndsSession()
    {
        const string token = "session token";
        var command = new EndSessionCommand(token);
        _mock.SetupUnregisterSessionByTokenAsyncReturnsCompletedTask(token);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        _mock.VerifyUnregisterSessionByTokenAsyncCalledOnce();
        _mock.VerifyCommitAsyncCalled(Times.Once);
    }
}
