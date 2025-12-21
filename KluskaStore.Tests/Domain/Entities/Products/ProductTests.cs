using KluskaStore.Domain.Entities.Products;

namespace KluskaStore.Tests.Domain.Entities.Products;

public class ProductTests
{
    private readonly Product _sut = new(
        new Dictionary<string, string>
        {
            ["size"] = "15cm x 30cm",
            ["weight"] = "5kg",
            ["category"] = "food"
        },
        100,
        "Sac of Rice"
    );

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsValid_ThenCreatesEntity()
    {
        var result = Product.Create(_sut.Specifications, _sut.Price, _sut.Name);

        result.IsSuccess.Should().BeTrue();
        result.Value.Specifications.Should().BeEquivalentTo(_sut.Specifications);
        result.Value.Price.Should().Be(_sut.Price);
        result.Value.Name.Should().Be(_sut.Name);
        result.Value.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public void GivenEntityCreation_WhenInitialDataIsInvalid_ThenReturnsFailure()
    {
        var result = Product.Create(new Dictionary<string, string>(), 0, null!);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void GivenSpecificationsPatch_ThenPatchesSpecifications()
    {
        var size = _sut.Specifications["size"];
        Dictionary<string, string?> patch = new()
        {
            ["category"] = null,
            ["weight"] = "3kg",
            ["brand"] = "joãozinho arrozes"
        };

        _sut.PatchSpecifications(patch);

        _sut.Specifications.ContainsKey("category").Should().BeFalse();
        _sut.Specifications["brand"].Should().Be(patch["brand"]);
        _sut.Specifications["weight"].Should().Be(patch["weight"]);
        _sut.Specifications["size"].Should().Be(size);
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
