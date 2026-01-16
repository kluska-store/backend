using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Users.AuthenticateUser;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.ValueObjects.AccountData;
using KluskaStore.Domain.ValueObjects.AccountData.Address;
using KluskaStore.Tests.Common.Builders;

namespace KluskaStore.Tests.Application.Features.Users.AuthenticateUser;

public class AuthenticateUserTests
{
    private readonly Mock<IUnitOfWork> _mock = new();
    private readonly AuthenticateUserHandler _sut;

    public AuthenticateUserTests() => _sut = new AuthenticateUserHandler(_mock.Object);

    private static AuthenticateUserCommand GenerateValidCommand(string? email = null, string? password = null)
        => new(email ?? "example@email.com", password ?? "password123");

    private void SetupGetUserByEmailReturnsNull() => _mock
        .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((User?)null);

    private User SetupGetUserByEmailReturnsValidUser()
    {
        var returnedUser = UserBuilder.Valid();
        _mock
            .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnedUser);

        return returnedUser;
    }

    private void SetupRegisterSessionReturnsSessionToken(string? token = null) => _mock
        .Setup(uow => uow.Sessions.RegisterAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(token ?? "session token");

    private void VerifyGetUserByEmailCalledOnce() => _mock.Verify(
        uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    private void VerifyCommitAsyncCalled(Func<Times> times) =>
        _mock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), times);

    private void VerifyRegisterSessionCalledOnce() => _mock.Verify(
        uow => uow.Sessions.RegisterAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

    private void VerifyRegisterSessionNotCalled() => _mock.Verify(
        uow => uow.Sessions.RegisterAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
        Times.Never
    );

    [Fact]
    public async Task GivenNonExistingUser_WhenTryingToAuthenticate_ThenReturnsFailure()
    {
        SetupGetUserByEmailReturnsNull();
        var command = GenerateValidCommand();

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AuthenticateUserErrors.UserNotFound.Code);
        VerifyGetUserByEmailCalledOnce();
        VerifyRegisterSessionNotCalled();
        VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenExistingUserAndWrongPassword_WhenTryingToAuthenticate_ThenReturnsFailure()
    {
        SetupGetUserByEmailReturnsValidUser();
        var command = GenerateValidCommand(password: "wrong password");

        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(AuthenticateUserErrors.PasswordIsIncorect.Code);
        VerifyGetUserByEmailCalledOnce();
        VerifyRegisterSessionNotCalled();
        VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenExistingUserAndCorrectPassword_WhenTryingToAuthenticate_ThenReturnsSessionToken()
    {
        var token = "session token";
        var user = SetupGetUserByEmailReturnsValidUser();
        var command = GenerateValidCommand(password: user.PasswordHash);
        SetupRegisterSessionReturnsSessionToken(token);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        VerifyGetUserByEmailCalledOnce();
        VerifyRegisterSessionCalledOnce();
        VerifyCommitAsyncCalled(Times.Once);
    }
}
