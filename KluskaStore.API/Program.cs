using KluskaStore.API.Extensions;
using KluskaStore.API.Middleware;
using KluskaStore.Application.Abstractions;
using KluskaStore.Infrastructure;
using KluskaStore.Infrastructure.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddInfrastructure(connectionString!);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Command<>).Assembly));

// Add Controllers and Endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KluskaStore API",
        Version = "v1",
        Description = "API RESTful do sistema de marketplace KluskaStore"
    })
);

// Build App
var app = builder.Build();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "KluskaStore API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MigrateDatabase<AppDbContext>();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();