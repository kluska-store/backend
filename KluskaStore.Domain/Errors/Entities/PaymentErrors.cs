namespace KluskaStore.Domain.Errors.Entities;

using static Error;

public static class PaymentErrors
{
    public static readonly Error EmptyUserId = Validation("Payment.EmptyUserId", "The User's Id must not be empty");
    public static readonly Error EmptyOrderId = Validation("Payment.EmptyOrderId", "The Order's Id must not be empty");

    public static readonly Error EmptyPayment =
        Validation("Payment.EmptyPayment", "The Payment must contain at least 1 Transference");

    public static readonly Error InvalidPaymentDate =
        Validation("Payment.InvalidPaymentDate", "The Payment's Date must be placed in the past");
}
