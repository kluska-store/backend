namespace KluskaStore.Application.Abstractions;

public record GuidQuery<TResp>(Guid Guid) : Query<TResp>;
