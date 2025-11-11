using KluskaStore.Application.Interfaces;

namespace KluskaStore.Application.DTOs.Store;

public record CreateStoreRequest(
  string Cnpj,
  string Name,
  string Email,
  string Password
) : IDto;