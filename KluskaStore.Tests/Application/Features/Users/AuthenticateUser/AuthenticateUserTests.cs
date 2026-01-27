using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Users.AuthenticateUser;
using KluskaStore.Tests.Common.Builders;
using KluskaStore.Tests.Common.Mocks.UnitOfWork;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Sessions;
using KluskaStore.Tests.Common.Mocks.UnitOfWork.Users;

namespace KluskaStore.Tests.Application.Features.Users.AuthenticateUser;

public class AuthenticateUserTests
{
    private readonly Mock<IUnitOfWork> _mock = new();
    private readonly AuthenticateUserHandler _sut;

    public AuthenticateUserTests() => _sut = new AuthenticateUserHandler(_mock.Object);

    private static AuthenticateUserCommand GenerateValidCommand(string? email = null, string? password = null)
        => new(email ?? "example@email.com", password ?? "password123");

    [Fact]
    public async Task GivenNonExistingUser_WhenTryingToAuthenticate_ThenReturnsFailure()
    {
        _mock.SetupGetUserByEmailReturnsNull();
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AuthenticateUserErrors.UserNotFound.Code);
        _mock.VerifyGetUserByEmailCalled(Times.Once);
        _mock.VerifyRegisterSessionCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenExistingUserAndWrongPassword_WhenTryingToAuthenticate_ThenReturnsFailure()
    {
        var user = UserBuilder.Valid();
        _mock.SetupGetUserByEmailReturns(user);
        var command = GenerateValidCommand(password: user.PasswordHash + ".");

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AuthenticateUserErrors.PasswordIsIncorrect.Code);
        _mock.VerifyGetUserByEmailCalled(Times.Once);
        _mock.VerifyRegisterSessionCalled(Times.Never);
        _mock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenExistingUserAndCorrectPassword_WhenTryingToAuthenticate_ThenReturnsSessionToken()
    {
        const string token = "session token";
        var user = UserBuilder.Valid();
        var command = GenerateValidCommand(email: user.Email.Value, password: user.PasswordHash);
        _mock.SetupGetUserByEmailReturns(user);
        _mock.SetupRegisterSessionReturns(token);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        _mock.VerifyGetUserByEmailCalled(Times.Once);
        _mock.VerifyRegisterSessionCalled(Times.Once);
        _mock.VerifyCommitAsyncCalled(Times.Once);
    }
}
