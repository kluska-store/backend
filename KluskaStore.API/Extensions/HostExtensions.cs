using Microsoft.EntityFrameworkCore;

namespace KluskaStore.API.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation("Migrations initialized...");

            var dbContext = services.GetRequiredService<TContext>();
            dbContext.Database.Migrate();

            logger.LogInformation("Migrations finished successfully");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Something went wrong during database migration");
        }

        return host;
    }
    
}