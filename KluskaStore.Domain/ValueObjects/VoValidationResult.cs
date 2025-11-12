using KluskaStore.Domain.Interfaces;

namespace KluskaStore.Domain.ValueObjects;

public record VoValidationResult<T>(bool IsValid, T? Vo, string? Error) where T : IValueObject;