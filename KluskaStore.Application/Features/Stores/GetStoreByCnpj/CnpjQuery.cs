using KluskaStore.Application.Abstractions;
using KluskaStore.Domain.ValueObjects;

namespace KluskaStore.Application.Features.Stores.GetStoreByCnpj;

public record CnpjQuery(Cnpj Cnpj) : Query<StoreResponse>;
