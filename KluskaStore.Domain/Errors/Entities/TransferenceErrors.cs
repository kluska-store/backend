namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class TransferenceErrors
{
    public static readonly Error EmptyReceiverId =
        Validation("Transference.EmptyReceiverId", "The Receiver Store's Id must not be empty");

    public static readonly Error InvalidTransference = Validation("Transference.InvalidTransference",
        "The Transference's price must be grater than 0");
}
