using KluskaStore.Application.Abstractions.Persistence;
using KluskaStore.Application.Features.Users.CreateUser;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Domain.Errors.ValueObjects;
using KluskaStore.Tests.Common.Mocks.UnitOfWork;
using KluskaStore.Tests.Common.Mocks.Users;

namespace KluskaStore.Tests.Application.Features.Users.CreateUser;

public class CreateUserTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IUserRepository> _userMock = new();
    private readonly CreateUserHandler _sut;

    public CreateUserTests() => _sut = new CreateUserHandler(_uowMock.Object, _userMock.Object);

    private static CreateUserCommand CreateValidCommand(
        string? cpf = null,
        string? email = null,
        string? username = null,
        string? phone = null,
        DateOnly? birthday = null,
        string? rawPassword = null
    ) => new(
        cpf ?? "01234567890",
        email ?? "example@email.com",
        username ?? "user",
        phone ?? "+1 (01) 91111-1111",
        birthday ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)),
        rawPassword ?? "password123"
    );

    [Fact]
    public async Task GivingValidData_WhenCreatingUser_ThenReturnsCreatedUsersId()
    {
        _userMock.SetupAddUserReturns(Task.CompletedTask);

        var command = CreateValidCommand();
        var result = await _sut.Handle(command);

        result.IsSuccess.Should().BeTrue();
        _userMock.VerifyAddUserCalled(Times.Once);
        _uowMock.VerifyCommitAsyncCalled(Times.Once);
    }

    [Fact]
    public async Task GivenInvalidCpf_WhenCreatingUser_ThenReturnsFailure()
    {
        var command = CreateValidCommand(cpf: "123");
        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(CpfErrors.InvalidCpf.Code);
        _userMock.VerifyAddUserCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenInvalidEmail_WhenCreatingUser_ThenReturnsFailure()
    {
        var command = CreateValidCommand(email: "invalid-email");
        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(EmailErrors.InvalidEmail.Code);
        _userMock.VerifyAddUserCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenNoUsername_WhenCreatingUser_ThenReturnsFailure()
    {
        var command = CreateValidCommand(username: "");
        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(UserErrors.EmptyUsername.Code);
        _userMock.VerifyAddUserCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenInvalidPhone_WhenCreatingUser_ThenReturnsFailure()
    {
        var command = CreateValidCommand(phone: "1234-5678");
        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(PhoneErrors.InvalidPhone.Code);
        _userMock.VerifyAddUserCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenInvalidBirthday_WhenCreatingUser_ThenReturnsFailure()
    {
        var command = CreateValidCommand(birthday: DateOnly.FromDateTime(DateTime.UtcNow));
        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(UserErrors.InvalidBirthday.Code);
        _userMock.VerifyAddUserCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }

    [Fact]
    public async Task GivenNoPassword_WhenCreatingUser_ThenReturnsFailure()
    {
        var command = CreateValidCommand(rawPassword: "");
        var result = await _sut.Handle(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(UserErrors.EmptyPassword.Code);
        _userMock.VerifyAddUserCalled(Times.Never);
        _uowMock.VerifyCommitAsyncCalled(Times.Never);
    }
}
