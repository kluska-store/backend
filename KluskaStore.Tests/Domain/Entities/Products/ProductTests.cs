using KluskaStore.Domain.Entities.Products;
using KluskaStore.Domain.Errors.Entities;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class ProductTests
{
    private readonly Product _sut = new(100, "Sac of Rice");

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesEntity()
    {
        var result = Product.Create(_sut.Price, _sut.Name);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Specifications.Should().BeEmpty();
        result.Value.Price.Should().Be(_sut.Price);
        result.Value.Name.Should().Be(_sut.Name);
        result.Value.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = Product.Create(0, null!);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void GivenSpecificationsPatch_WhenTargetSpecificationDoesNotExist_ThenCreatesSpecification()
    {
        const string target = "size", value = "10 x 20cm";
        _sut.Specifications.ContainsKey(target).Should().BeFalse();

        _sut.PatchSpecifications(new Dictionary<string, string?> { [target] = value });

        _sut.Specifications[target].Should().Be(value);
    }

    [Fact]
    public void GivenSpecificationPatch_WhenTargetSpecificationExists_ThenAltersItsValue()
    {
        const string target = "size", value = "10 x 20cm";
        var patch = new Dictionary<string, string?> { [target] = "2 x 30cm" };
        _sut.PatchSpecifications(patch);

        patch[target] = value;
        _sut.PatchSpecifications(patch);

        _sut.Specifications[target].Should().Be(value);
    }

    [Fact]
    public void GivenSpecificationPatch_WhenTargetSpecificationIsSetToNull_ThenRemovesSpecification()
    {
        const string target = "size";
        var patch = new Dictionary<string, string?> { [target] = "10 x 20cm" };
        _sut.PatchSpecifications(patch);

        patch[target] = null;
        _sut.PatchSpecifications(patch);

        _sut.Specifications.ContainsKey(target).Should().BeFalse();
    }

    [Fact]
    public void GivenPriceChange_WhenNewPriceIsValid_ThenChangesPrice()
    {
        var newPrice = 15;
        var result = _sut.ChangePrice(newPrice);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(_sut);
        _sut.Price.Should().Be(newPrice);
    }

    [Fact]
    public void GivenPriceChange_WhenNewPriceIsInvalid_ThenReturnsFailure()
    {
        var result = _sut.ChangePrice(0);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ProductErrors.InvalidPrice.Code);
    }

    [Fact]
    public void GivenNameChange_WhenNewNameIsValid_ThenChangesName()
    {
        var newName = "Sac of Premium Rice";
        var result = _sut.ChangeName(newName);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeSameAs(_sut);
        _sut.Name.Should().Be(newName);
    }

    [Fact]
    public void GivenNameChange_WhenNewNameIsInvalid_ThenReturnsFailure()
    {
        var result = _sut.ChangeName("");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ProductErrors.EmptyName.Code);
    }

    [Fact]
    public void GivenDescriptionChange_ThenChangesDescription()
    {
        var newDescription = "Rice from the furthest end of the Milky Road";
        _sut.ChangeDescription(newDescription);

        _sut.Description.Should().Be(newDescription);
    }

    [Fact]
    public void GivenMarkingAsUnavailable_WhenAvailable_ThenSetsIsAvailableToFalse()
    {
        _sut.IsAvailable.Should().BeTrue();

        _sut.MarkAsUnavailable();

        _sut.IsAvailable.Should().BeFalse();
    }

    [Fact]
    public void GivenMarkingAsAvailable_WhenUnavailable_ThenSetsIsAvailableToTrue()
    {
        _sut.MarkAsUnavailable();

        _sut.MarkAsAvailable();

        _sut.IsAvailable.Should().BeTrue();
    }
}
