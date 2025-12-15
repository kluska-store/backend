using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Sessions.VerifySession;
using KluskaStore.Domain.Entities.Accounts;
using OwnerType = KluskaStore.Domain.ValueObjects.SessionOwner.OwnerTypeEnum;

namespace KluskaStore.Tests.Application.Features.Sessions.VerifySession;

public class VerifySessionTests
{
    private readonly Mock<ISessionRepository> _mock = new();
    private readonly VerifySessionHandler _sut;

    public VerifySessionTests() => _sut = new VerifySessionHandler(_mock.Object);

    private static Session GenerateValidSession(
        OwnerType ownerType = OwnerType.User,
        Guid? ownerId = null,
        DateTime? createdAt = null
    )
    {
        ownerId ??= Guid.NewGuid();
        createdAt ??= DateTime.UtcNow;
        var result = ownerType == OwnerType.Store
            ? Session.CreateStoreSession((Guid)ownerId, (DateTime)createdAt)
            : Session.CreateUserSession((Guid)ownerId, (DateTime)createdAt);
        return result.Value;
    }

    private void SetupGetBySessionTokenReturnsNull() => _mock
        .Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Session?)null);

    private void SetupGetBySessionTokenReturnsValidSession() => _mock
        .Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(GenerateValidSession());

    private void SetupGetBySessionTokenReturnsExpiredSession() => _mock
        .Setup(repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(GenerateValidSession(createdAt: DateTime.UtcNow.AddMonths(-3)));

    private void VerifyGetBySessionTokenCalledOnce() => _mock.Verify(
        repo => repo.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    [Fact]
    public async Task GivenNonRegisteredSession_WhenVerifyingSessionValidity_ThenReturnsFalse()
    {
        SetupGetBySessionTokenReturnsNull();
        var query = new VerifySessionQuery("session token");

        var isValid = await _sut.Handle(query);

        isValid.Should().BeFalse();
        VerifyGetBySessionTokenCalledOnce();
    }

    [Fact]
    public async Task GivenExpiredSession_WhenVerifyingSessionValidity_ThenReturnsFalse()
    {
        SetupGetBySessionTokenReturnsExpiredSession();
        var query = new VerifySessionQuery("session token");

        var isValid = await _sut.Handle(query);

        isValid.Should().BeFalse();
        VerifyGetBySessionTokenCalledOnce();
    }

    [Fact]
    public async Task GivenValidSession_WhenVerifyingSessionValidity_ThenReturnsTrue()
    {
        SetupGetBySessionTokenReturnsValidSession();
        var query = new VerifySessionQuery("session token");

        var isValid = await _sut.Handle(query);

        isValid.Should().BeTrue();
        VerifyGetBySessionTokenCalledOnce();
    }
}
