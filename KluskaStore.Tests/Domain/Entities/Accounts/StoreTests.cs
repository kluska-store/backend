using System.ComponentModel.Design.Serialization;
using KluskaStore.Domain.Entities.Accounts;
using KluskaStore.Domain.Errors.Entities;
using KluskaStore.Domain.ValueObjects.AccountData.Address;
using KluskaStore.Tests.Common.Builders;
using Cnpj = KluskaStore.Domain.ValueObjects.AccountData.Cnpj;
using Email = KluskaStore.Domain.ValueObjects.AccountData.Email;
using Phone = KluskaStore.Domain.ValueObjects.AccountData.Phone;
using PostalCode = KluskaStore.Domain.ValueObjects.AccountData.Address.PostalCode;

namespace KluskaStore.Tests.Domain.Entities.Accounts;

public class StoreTests
{
    private readonly Store _sut = StoreBuilder.Valid();

    [Fact]
    public void GivenEntityCreation_WhenDataIsValid_ThenCreatesStore()
    {
        var result = Store.Create(_sut.Cnpj, _sut.Name, _sut.Email, _sut.PasswordHash, _sut.Address);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.IsActive.Should().BeTrue();
        result.Value.Cnpj.Should().Be(_sut.Cnpj);
        result.Value.Name.Should().Be(_sut.Name);
        result.Value.Email.Should().Be(_sut.Email);
        result.Value.PasswordHash.Should().Be(_sut.PasswordHash);
        result.Value.Address.Should().Be(_sut.Address);
        result.Value.Phones.Should().BeEmpty();
    }

    [Fact]
    public void GivenEntityCreation_WhenNameIsEmpty_ThenReturnsFailure()
    {
        var result = Store.Create(_sut.Cnpj, "", _sut.Email, _sut.PasswordHash, _sut.Address);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(StoreErrors.EmptyName.Code);
    }

    [Fact]
    public void GivenEntityCreation_WhenPasswordIsEmpty_ThenReturnsFailure()
    {
        var result = Store.Create(_sut.Cnpj, _sut.Name, _sut.Email, "", _sut.Address);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(StoreErrors.EmptyPassword.Code);
    }

    [Fact]
    public void GivenNameChange_WhenValidNewName_ThenChangesName()
    {
        var newName = "new name";
        var result = _sut.ChangeName("new name");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(_sut);
        _sut.Name.Should().Be(newName);
    }

    [Fact]
    public void GivenNameChange_WhenInvalidNewName_ThenReturnsFailure()
    {
        var result = _sut.ChangeName("");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(StoreErrors.EmptyName.Code);
    }

    [Fact]
    public void GivenPictureUrlChange_ThenChangesPictureUrl()
    {
        var newUrl = "https://example.com";
        _sut.ChangePicture(newUrl);

        _sut.PictureUrl.Should().Be(newUrl);
    }

    [Fact]
    public void GivenPasswordChange_WhenValidNewPassword_ThenChangesPassword()
    {
        var newPassword = "password123";
        var result = _sut.ChangePasswordHash(newPassword);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(_sut);
        _sut.PasswordHash.Should().Be(newPassword);
    }

    [Fact]
    public void GivenPasswordChange_WhenInvalidNewPassword_ThenReturnsFailure()
    {
        var result = _sut.ChangePasswordHash("");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(StoreErrors.EmptyPassword.Code);
    }

    [Fact]
    public void GivenAddressChange_ThenChangesAddress()
    {
        var newAddress = AddressBuilder.Valid();
        _sut.ChangeAddress(newAddress);

        _sut.Address.Should().Be(newAddress);
    }

    [Fact]
    public void GivenDeactivationAndReactivation_ThenDeactivatesAndReactivates()
    {
        _sut.IsActive.Should().BeTrue();

        _sut.Deactivate();
        _sut.IsActive.Should().BeFalse();

        _sut.Activate();
        _sut.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GivenPhoneAdditionAndRemoval_ThenAddsAndRemovesPhone()
    {
        var newPhone = PhoneBuilder.Valid();
        _sut.AddPhones(newPhone);
        _sut.Phones[^1].Should().Be(newPhone);

        _sut.RemovePhone(newPhone);
        _sut.Phones.Should().NotContain(newPhone);
    }
}
