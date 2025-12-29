using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Domain.ValueObjects.AccountData.Address;
using Cpf = KluskaStore.Domain.ValueObjects.AccountData.Cpf;
using Email = KluskaStore.Domain.ValueObjects.AccountData.Email;
using Phone = KluskaStore.Domain.ValueObjects.AccountData.Phone;

namespace KluskaStore.Tests.Domain.Entities.Accounts;

public class UserTests
{
    private readonly User _sut = new(
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
        result.Value.Should().NotBeNull();
        result.Value.IsActive.Should().BeTrue();
        result.Value.Cpf.Should().Be(_sut.Cpf);
        result.Value.Email.Should().Be(_sut.Email);
        result.Value.Username.Should().Be(_sut.Username);
        result.Value.Phone.Should().Be(_sut.Phone);
        result.Value.Birthday.Should().Be(_sut.Birthday);
        result.Value.PasswordHash.Should().Be(_sut.PasswordHash);
        result.Value.Addresses.Should().BeEmpty();
        result.Value.HasAnyAddress().Should().BeFalse();
    }

    [Fact]
    public void GivenEntityCreation_WhenDataIsInvalid_ThenReturnsFailure()
    {
        var result = User.Create(null!, null!, "", null!, DateOnly.FromDateTime(DateTime.UtcNow), "");

        result.IsFailure.Should().BeTrue();
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
        _sut.Username.Should().Be(newName);
    }

    [Fact]
    public void GivenUsernameChange_WhenNewUsernameIsInvalid_ThenReturnsFailure()
    {
        var result = _sut.ChangeUsername("");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(UserErrors.EmptyUsername.Code);
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
        result.Value.Should().BeSameAs(_sut);
        _sut.Birthday.Should().Be(newBirthday);
    }

    [Fact]
    public void GivenBirthdayChange_WhenNewBirthdayIsInvalid_ThenReturnsFailure()
    {
        var result = _sut.ChangeBirthday(DateOnly.FromDateTime(DateTime.UtcNow));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(UserErrors.InvalidBirthday.Code);
    }

    [Fact]
    public void GivenPasswordChange_WhenNewPasswordIsValid_ThenChangesPassword()
    {
        var newPassword = "password 2";
        var result = _sut.ChangePassword(newPassword);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(_sut);
        _sut.PasswordHash.Should().Be(newPassword);
    }

    [Fact]
    public void GivenPasswordChange_WhenNewPasswordIsInvalid_ThenReturnsFailure()
    {
        var result = _sut.ChangePassword("");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(UserErrors.EmptyPassword.Code);
    }

    [Fact]
    public void GivenAddressAddition_ThenIncreasesTheAmountOfAddresses()
    {
        var address = new Address(null!, null!, null!, null!, 3, null!, null!);
        _sut.Addresses.Should().BeEmpty();

        _sut.AddAddresses(address);

        _sut.Addresses.Should().ContainEquivalentOf(address);
    }

    [Fact]
    public void GivenAddressRemoval_WhenRemovingThroughTheInstance_ThenDecreasesTheAmountOfAddresses()
    {
        var address = new Address(null!, null!, null!, null!, 3, null!, null!);
        _sut.AddAddresses(null!, address, null!);
        var lastAmount = _sut.Addresses.Count;

        _sut.RemoveAddresses(address);

        _sut.Addresses.Count.Should().Be(lastAmount - 1);
        _sut.Addresses.Should().NotContainEquivalentOf(address);
    }

    [Fact]
    public void GivenAddressRemoval_WhenRemovingThroughTheIndex_ThenDecreasesTheAmountOfAddresses()
    {
        _sut.AddAddresses(null!, null!, null!);
        var lastAmount = _sut.Addresses.Count;

        _sut.RemoveAddressAt(1);

        _sut.Addresses.Count.Should().Be(lastAmount - 1);
    }

    [Fact]
    public void GivenClearAddressesCall_ThenClearsAllAddresses()
    {
        _sut.ClearAddresses();

        _sut.Addresses.Should().BeEmpty();
    }
}
