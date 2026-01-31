using KluskaStore.Application.Abstractions;
using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Users.AuthenticateUser;
using KluskaStore.Tests.Common.Builders;
using KluskaStore.Tests.Common.Mocks.UnitOfWork;
using KluskaStore.Tests.Common.Mocks.Sessions;
using KluskaStore.Tests.Common.Mocks.SessionTokenGenerator;
using KluskaStore.Tests.Common.Mocks.Users;

namespace KluskaStore.Tests.Application.Features.Users.AuthenticateUser;

public class AuthenticateUserTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IUserRepository> _userMock = new();
    private readonly Mock<ISessionRepository> _sessionMock = new();
    private readonly Mock<ISessionTokenGenerator> _tokenGeneratorMock = new();
    private readonly AuthenticateUserHandler _sut;

    public AuthenticateUserTests() => _sut = new AuthenticateUserHandler(
        _uowMock.Object,
        _userMock.Object,
        _sessionMock.Object,
        _tokenGeneratorMock.Object
    );

    private static AuthenticateUserCommand GenerateValidCommand(string? email = null, string? password = null)
        => new(email ?? "example@email.com", password ?? "password123");

    [Fact]
    public async Task GivenNonExistingUser_WhenTryingToAuthenticate_ThenReturnsFailure()
    {
        _userMock.SetupGetUserByEmailReturnsNull();
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AuthenticateUserErrors.UserNotFound.Code);
        _userMock.VerifyGetUserByEmailCalled(Times.Once);
        _sessionMock.VerifyRegisterSessionCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
        _tokenGeneratorMock.VerifyGenerateNewTokenCalled(Times.Never);
    }

    [Fact]
    public async Task GivenExistingUserAndWrongPassword_WhenTryingToAuthenticate_ThenReturnsFailure()
    {
        var user = UserBuilder.Valid();
        _userMock.SetupGetUserByEmailReturns(user);
        var command = GenerateValidCommand(password: user.PasswordHash + ".");

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AuthenticateUserErrors.PasswordIsIncorrect.Code);
        _userMock.VerifyGetUserByEmailCalled(Times.Once);
        _sessionMock.VerifyRegisterSessionCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
        _tokenGeneratorMock.VerifyGenerateNewTokenCalled(Times.Never);
    }

    [Fact]
    public async Task GivenExistingUserAndCorrectPassword_WhenTryingToAuthenticate_ThenReturnsSessionToken()
    {
        var user = UserBuilder.WithId(Guid.NewGuid());
        var session = UserSessionBuilder.WithUserId(userId: user.Id);
        var command = GenerateValidCommand(email: user.Email.Value, password: user.PasswordHash);
        _userMock.SetupGetUserByEmailReturns(user);
        _sessionMock.SetupRegisterSessionReturns(Task.CompletedTask);
        _tokenGeneratorMock.SetupGenerateNewTokenReturns(session.Token);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(session.Token);
        _userMock.VerifyGetUserByEmailCalled(Times.Once);
        _sessionMock.VerifyRegisterSessionCalled(Times.Once);
        _uowMock.VerifyCommitAsyncCalled(Times.Once);
        _tokenGeneratorMock.VerifyGenerateNewTokenCalled(Times.Once);
    }
}
