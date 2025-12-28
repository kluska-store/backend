namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class StoreErrors
{
    public static readonly Error EmptyName = Validation("Store.EmptyName", "The Name must not be empty");
    public static readonly Error EmptyPassword = Validation("Store.EmptyPassword", "The Password must not be empty");
}
