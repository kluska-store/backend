namespace KluskaStore.Application.Features.Sessions.GetSessionByToken;

public record GetSessionByTokenResponse(string Token, Guid OwnerId, DateTime ExpiresAt, SessionOwnerType OwnerType);
