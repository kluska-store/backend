using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Domain.Interfaces;

public interface ICreatableVo<TVo> where TVo : IValueObject {
  static abstract VoValidationResult<TVo> New(string value);
}