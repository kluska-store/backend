namespace KluskaStore.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        string errorMessage;
        try
        {
            await next(context);
            return;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            errorMessage = "Internal server error";
        }

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = errorMessage });
    }
}
