using KluskaStore.Application.Interfaces;

namespace KluskaStore.Application.DTOs;

public record GuidRequest(Guid Guid) : IPrimitiveRequest<Guid>;