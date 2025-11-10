using KluskaStore.Domain.Repositories;
using KluskaStore.Infrastructure.Data;
using KluskaStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Controllers and Endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerGen(options => 
  options.SwaggerDoc("v1", new OpenApiInfo {
    Title = "KluskaStore API",
    Version = "v1",
    Description = "API RESTful do sistema de marketplace KluskaStore"
  })
);

// Build App
var app = builder.Build();

// Configure Swagger
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "KluskaStore API v1");
    options.RoutePrefix = string.Empty;
  });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();