using KluskaStore.Domain.Entities;
using KluskaStore.Domain.Shared;
using MediatR;

namespace KluskaStore.Application.Features.Stores.CreateStore;

public record CreateStoreCommand(
  string Cnpj,
  string Name,
  string Email,
  string Password
) : IRequest<Result<StoreResponse>>;