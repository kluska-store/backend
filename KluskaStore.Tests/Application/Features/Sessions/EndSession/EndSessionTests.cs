using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Sessions.EndSession;

namespace KluskaStore.Tests.Application.Features.Sessions.EndSession;

public class EndSessionTests
{
    private readonly Mock<ISessionRepository> _mock = new();
    private readonly EndSessionHandler _sut;

    public EndSessionTests() => _sut = new EndSessionHandler(_mock.Object);

    [Fact]
    public async Task GivenSessionEnd_ThenEndsSession()
    {
        var command = new EndSessionCommand("session token");
        _mock.Setup(repo => repo.UnregisterAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _sut.Handle(command);

        _mock.Verify(
            repo => repo.UnregisterAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
