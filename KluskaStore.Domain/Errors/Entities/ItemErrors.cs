namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class ItemErrors
{
    public static readonly Error EmptyItem =
        Validation("Item.EmptyItem", "The item cannot be created with quantity = 0");
}
