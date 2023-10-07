using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.

// Criar uma variável que armazena a string de conexão com o banco de dados MySQL.
var connectionStringMysql = builder.Configuration.GetConnectionString("ConnectionMysql");

// Adicionar um serviço do tipo MySQLContext ao objeto builder.Services.
builder.Services.AddDbContext<MySQLContext>(option => option.UseMySql(
    connectionStringMysql, // Usar a string de conexão.
    ServerVersion.AutoDetect(connectionStringMysql), // Especificar a versão do servidor MySQL.
    builder => builder.MigrationsAssembly("API") // Especifica o assembly do projeto que contém as classes de migrações do EF Core.
    )
);


builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, JsonPatchSample.MyJPIF.GetJsonPatchInputFormatter());
});

builder.Services.AddControllers().AddNewtonsoftJson();

// Configurar os serviços relacionados aos controladores.
builder.Services.AddControllers();

// Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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
