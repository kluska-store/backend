using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Sessions.GetSessionByToken;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Application.Features.Sessions.GetSessionByToken;

public class GetSessionByTokenTests
{
    private readonly Mock<ISessionRepository> _mock = new();
    private readonly GetSessionByTokenHandler _sut;

    public GetSessionByTokenTests() => _sut = new GetSessionByTokenHandler(_mock.Object);

    private static Session GenerateValidSession(DateTime? createdAt = null)
        => new("token", new SessionOwner(SessionOwner.OwnerTypeEnum.User, Guid.NewGuid()), createdAt ?? DateTime.UtcNow);

    private void SetupGetSessionByTokenReturnsNull() => _mock
        .Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Session?)null);

    private void SetupGetSessionByTokenReturnsValidSession() => _mock
        .Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(GenerateValidSession());

    private void SetupGetSessionByTokenReturnsExpiredSession() => _mock
        .Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(GenerateValidSession(createdAt: DateTime.UtcNow.AddMonths(-3)));

    private void VerifyGetSessionByTokenCalledOnce() => _mock.Verify(
        repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    [Fact]
    public async Task GivenNonExistingSession_WhenGettingByToken_ThenReturnsNull()
    {
        SetupGetSessionByTokenReturnsNull();
        var query = new GetSessionByTokenQuery("session token");

        var response = await _sut.Handle(query);

        response.Should().BeNull();
        VerifyGetSessionByTokenCalledOnce();
    }

    [Fact]
    public async Task GivenExistingExpiredSession_WhenGettingByToken_ThenReturnsNull()
    {
        SetupGetSessionByTokenReturnsExpiredSession();
        var query = new GetSessionByTokenQuery("session token");

        var response = await _sut.Handle(query);

        response.Should().BeNull();
        VerifyGetSessionByTokenCalledOnce();
    }

    [Fact]
    public async Task GivenExistingNonExpiredSession_WhenGettingByToken_ThenReturnsResponse()
    {
        SetupGetSessionByTokenReturnsValidSession();
        var query = new GetSessionByTokenQuery("session token");

        var response = await _sut.Handle(query);

        response.Should().NotBeNull();
        VerifyGetSessionByTokenCalledOnce();
    }
}
