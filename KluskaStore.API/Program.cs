using KluskaStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAÇÃO DO EF CORE ---
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// --- CONFIGURAÇÃO DE CONTROLLERS E SWAGGER ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
  options.SwaggerDoc("v1", new OpenApiInfo {
    Title = "KluskaStore API",
    Version = "v1",
    Description = "API RESTful do sistema de marketplace KluskaStore"
  })
);

var app = builder.Build();

// --- CONFIGURAÇÃO DO PIPELINE ---
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