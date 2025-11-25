using FluentAssertions;
using KluskaStore.Domain.Entities;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Tests.Domain.Entities;

public class UserTests
{
    private readonly User _sut = new User(
        new Cpf("cpf"),
        new Email("email"),
        "username",
        new Phone("phone"),
        DateOnly.Parse("2000-03-03"),
        "password"
    );

    [Fact]
    public void GivenEntityCreation_WhenDataIsValid_ThenCreatesUser()
    {
        var result = User.Create(_sut.Cpf, _sut.Email, _sut.Username, _sut.Phone, _sut.Birthday, _sut.PasswordHash);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeAssignableTo<User>();
        result.Value.IsActive.Should().BeTrue();
        result.Value.Cpf.Should().Be(_sut.Cpf);
        result.Value.Email.Should().Be(_sut.Email);
        result.Value.Username.Should().Be(_sut.Username);
        result.Value.Phone.Should().Be(_sut.Phone);
        result.Value.Birthday.Should().Be(_sut.Birthday);
        result.Value.PasswordHash.Should().Be(_sut.PasswordHash);
    }

    [Fact]
    public void GivenEntityCreation_WhenDataIsInvalid_ThenReturnsFailure()
    {
        var result = User.Create(null!, null!, "", null!, DateOnly.FromDateTime(DateTime.UtcNow), "");

        result.IsFailure.Should().BeTrue();
        result.Errors.Count.Should().Be(3);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void GivenEmailChange_ThenChangesEmail()
    {
        var newEmail = new Email("email 2");
        _sut.ChangeEmail(newEmail);

        _sut.Email.Should().Be(newEmail);
    }

    [Fact]
    public void GivenUsernameChange_WhenNewUsernameIsValid_ThenChangesUsername()
    {
        var newName = "new name";
        var result = _sut.ChangeUsername(newName);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeSameAs(_sut);
        _sut.Username.Should().Be(newName);
    }

    [Fact]
    public void GivenUsernameChange_WhenNewUsernameIsInvalid_ThenReturnsFailure()
    {
        var result = _sut.ChangeUsername("");

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void GivenUserDeactivationAndReactivation_ThenDeactivatesAndReactivatesUser()
    {
        _sut.IsActive.Should().BeTrue();

        _sut.Deactivate();
        _sut.IsActive.Should().BeFalse();

        _sut.Activate();
        _sut.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenPhoneChange_ThenChangesPhone()
    {
        var newPhone = new Phone("phone 2");
        _sut.ChangePhone(newPhone);

        _sut.Phone.Should().Be(newPhone);
    }

    [Fact]
    public void GivenBirthdayChange_WhenNewBirthdayIsValid_ThenChangesBirthday()
    {
        var newBirthday = DateOnly.Parse("2000-03-03");
        var result = _sut.ChangeBirthday(newBirthday);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeSameAs(_sut);
        _sut.Birthday.Should().Be(newBirthday);
    }

    [Fact]
    public void GivenBirthdayChange_WhenNewBirthdayIsInvalid_ThenReturnsFailure()
    {
        var result = _sut.ChangeBirthday(DateOnly.FromDateTime(DateTime.UtcNow));

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void GivenPasswordChange_WhenNewPasswordIsValid_ThenChangesPassword()
    {
        var newPassword = "password 2";
        var result = _sut.ChangePassword(newPassword);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().BeSameAs(_sut);
        _sut.PasswordHash.Should().Be(newPassword);
    }

    [Fact]
    public void GivenPasswordChange_WhenNewPasswordIsInvalid_ThenReturnsFailure()
    {
        var result = _sut.ChangePassword("");

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}
