using App.Application.Interfaces;
using App.Application.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddControllers();
// Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Configuração do SwaggerGen para gerar a documentação da Web API.
builder.Services.AddSwaggerGen(
    c =>
    {
        // Habilita o uso de anotações (como [SwaggerOperation]) para melhorar a documentação.
        c.EnableAnnotations();
        // Define a versão da documentação Swagger como "v1".
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Swagger Documentação Web API",
            Description = "Um exemplo de como fornecer a documentação para APIs.",
            Contact = new OpenApiContact() { Name = "Tech Challenge - Grupo 13", Email = "grupo13@fiap.com" },
            License = new OpenApiLicense() { Name = "MIT License", Url = new Uri("https://opensource.org/licenses/MIT") }
        });
    }
);

var app = builder.Build();

// Configure a pipeline de solicitação HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseReDoc(c =>
    {
        c.DocumentTitle = "REDOC API Documentation";
        c.SpecUrl = "/swagger/v1/swagger.json";
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
