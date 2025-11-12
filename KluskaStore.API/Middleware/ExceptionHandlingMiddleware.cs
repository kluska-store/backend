using KluskaStore.Application.Exceptions;

namespace KluskaStore.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {
  public async Task InvokeAsync(HttpContext context) {
    try {
      await next(context);
    }
    catch (NotFoundException e) {
      logger.LogWarning(e, "Application exception caught: Not Found");
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      await HandleExceptionAsync(context, e.Message);
    }
    catch (BadRequestException e) {
      logger.LogWarning(e, "Application exception caught: Bad Request");
      context.Response.StatusCode = StatusCodes.Status400BadRequest;
      await HandleExceptionAsync(context, e.Message);
    }
    catch (Exception e) {
      logger.LogError(e, "Unexpected error");
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      await HandleExceptionAsync(context, "Internal server error");
    }
  }

  private static async Task HandleExceptionAsync(HttpContext context, string message) {
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsJsonAsync(new { error = message });
  }
}