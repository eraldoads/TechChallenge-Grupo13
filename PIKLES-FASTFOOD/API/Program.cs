using Application.Services;
using Data.Context;
using Data.Repository;
using Domain.Port.DrivenPort;
using Domain.Port.DriverPort;
using Domain.Port.Services;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// Cria um builder de aplicação web com os argumentos passados.
var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.

// Cria uma variável que armazena a string de conexão com o banco de dados MySQL.
var connectionStringMysql = builder.Configuration.GetConnectionString("ConnectionMysql");

// Adiciona um serviço do tipo MySQLContext ao objeto builder.Services.
builder.Services.AddDbContext<MySQLContext>(option => option.UseMySql(
    connectionStringMysql, // Usa a string de conexão.
    ServerVersion.AutoDetect(connectionStringMysql), // Especifica a versão do servidor MySQL.
    builder => builder.MigrationsAssembly("API") // Especifica o assembly do projeto que contém as classes de migrações do EF Core.
));

// Adiciona os serviços de controllers ao builder.
builder.Services.AddControllers(options =>
{
    // Insere um formato de entrada personalizado para o JsonPatch.
    options.InputFormatters.Insert(0, JsonPatchSample.MyJPIF.GetJsonPatchInputFormatter());
});

// Adiciona os serviços específicos ao contêiner.
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

// Adiciona os repositórios específicos ao contêiner.
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// Adiciona o suporte ao NewtonsoftJson aos controllers.
builder.Services.AddControllers().AddNewtonsoftJson();

// Configura as opções de rota para usar URLs e query strings em minúsculo.
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Configura os serviços relacionados aos controladores.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(AjustaDataHoraLocal));
}).AddNewtonsoftJson(options =>
{
    // Usa a formatação padrão (PascalCase) para as propriedades.
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

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
            Title = "Tech Challenge - Grupo 13 - Fase I",
            Description = "Documentacao dos endpoints da API.",
            Contact = new OpenApiContact() { Name = "Tech Challenge - Grupo 13", Email = "grupo13@fiap.com" },
            License = new OpenApiLicense() { Name = "MIT License", Url = new Uri("https://opensource.org/licenses/MIT") },
            Version = "1.0.11"
        });

        // Habilita o uso para registrar o SchemaFilter.
        c.SchemaFilter<ClienteSchemaFilter>();
    }
);

var app = builder.Build();

// Configura a pipeline de solicitação HTTP.
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

// Adiciona o middleware de autorização à pipeline de solicitação HTTP.
// Este middleware é responsável por garantir que o usuário esteja autorizado a acessar os recursos solicitados.
app.UseAuthorization();

// Adiciona o middleware de roteamento de controladores à pipeline de solicitação HTTP.
// Este middleware é responsável por rotear as solicitações HTTP para os controladores apropriados.
app.MapControllers();

// Inicia a execução da aplicação.
// Este método bloqueia o thread chamado e aguarda até que a aplicação seja encerrada.
app.Run();
