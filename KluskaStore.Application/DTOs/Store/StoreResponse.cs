using KluskaStore.Application.Interfaces;

namespace KluskaStore.Application.DTOs.Store;

public record StoreResponse(
  Guid Id,
  string Cnpj,
  string Name,
  string Email,
  bool IsActive
) : IDto;