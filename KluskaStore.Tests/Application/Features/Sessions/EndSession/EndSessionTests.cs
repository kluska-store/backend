using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Sessions.EndSession;
using KluskaStore.Tests.Common.Builders;
using KluskaStore.Tests.Common.Mocks.UnitOfWork;
using KluskaStore.Tests.Common.Mocks.Sessions;

namespace KluskaStore.Tests.Application.Features.Sessions.EndSession;

public class EndSessionTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<ISessionRepository> _sessionMock = new();
    private readonly EndSessionHandler _sut;

    public EndSessionTests() => _sut = new EndSessionHandler(_uowMock.Object, _sessionMock.Object);

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GivenExistingSession_WhenTryingToEndIt_ThenEndsSession(bool isUserSession)
    {
        var session = SessionBuilder.Valid(isUserSession);
        var command = new EndSessionCommand(session.Token);
        _sessionMock.SetupGetSessionByTokenReturns(session);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _sessionMock.VerifyUnregisterSessionByTokenAsyncCalled(Times.Once);
        _uowMock.VerifyCommitAsyncCalled(Times.Once);
    }

    [Fact]
    public async Task GivenNonExistingSession_WhenTryingToEndIt_ThenEndsSession()
    {
        var command = new EndSessionCommand("session token");
        _sessionMock.SetupGetSessionByTokenReturnsNull();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        _sessionMock.VerifyGetSessionByTokenCalled(Times.Once);
        _sessionMock.VerifyUnregisterSessionByTokenAsyncCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }
}
