using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Users.AuthenticateUser;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.ValueObjects.AccountData;
using KluskaStore.Domain.ValueObjects.AccountData.Address;

namespace KluskaStore.Tests.Application.Features.Users.AuthenticateUser;

public class AuthenticateUserTests
{
    private readonly Mock<IUnitOfWork> _mock = new();
    private readonly AuthenticateUserHandler _sut;

    public AuthenticateUserTests() => _sut = new AuthenticateUserHandler(_mock.Object);

    private static AuthenticateUserCommand GenerateValidCommand(string? email = null, string? password = null)
        => new(email ?? "example@email.com", password ?? "password123");

    private static User GenerateValidUser(
        string? cpf = null,
        string? email = null,
        string? username = null,
        string? phone = null,
        DateOnly? birthday = null,
        string? password = null,
        IEnumerable<Address>? addresses = null
    ) => new(
        new Cpf(cpf ?? "00000000000"),
        new Email(email ?? "example@email.com"),
        username ?? "username",
        new Phone(phone ?? "+55 (11) 00000-0000"),
        birthday ?? DateOnly.FromDateTime(DateTime.UtcNow),
        password ?? "password123",
        addresses ?? []
    );

    private void SetupGetUserByEmailReturnsNull() => _mock
        .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((User?)null);

    private void SetupGetUserByEmailReturnsValidUser() => _mock
        .Setup(uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(GenerateValidUser());

    private void SetupRegisterSessionReturnsSessionToken(string? token = null) => _mock
        .Setup(uow => uow.Sessions.RegisterAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(token ?? "session token");

    private void VerifyGetUserByEmailCalledOnce() => _mock.Verify(
        uow => uow.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
        Times.Once
    );

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
    }

    [Fact]
    public async Task GivenExistingUserAndCorrectPassword_WhenTryingToAuthenticate_ThenReturnsSessionToken()
    {
        var token = "session token";
        var command = GenerateValidCommand();
        SetupGetUserByEmailReturnsValidUser();
        SetupRegisterSessionReturnsSessionToken(token);

        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        VerifyGetUserByEmailCalledOnce();
        VerifyRegisterSessionCalledOnce();
    }
}
