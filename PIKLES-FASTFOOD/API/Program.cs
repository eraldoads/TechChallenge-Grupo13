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

// Cria um builder de aplica��o web com os argumentos passados.
var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner.

// Cria uma vari�vel que armazena a string de conex�o com o banco de dados MySQL.
var connectionStringMysql = builder.Configuration.GetConnectionString("ConnectionMysql");

// Adiciona um servi�o do tipo MySQLContext ao objeto builder.Services.
builder.Services.AddDbContext<MySQLContext>(option => option.UseMySql(
    connectionStringMysql, // Usa a string de conex�o.
    ServerVersion.AutoDetect(connectionStringMysql), // Especifica a vers�o do servidor MySQL.
    builder => builder.MigrationsAssembly("API") // Especifica o assembly do projeto que cont�m as classes de migra��es do EF Core.
));

// Adiciona os servi�os de controllers ao builder.
builder.Services.AddControllers(options =>
{
    // Insere um formato de entrada personalizado para o JsonPatch.
    options.InputFormatters.Insert(0, JsonPatchSample.MyJPIF.GetJsonPatchInputFormatter());
});

// Adiciona os servi�os espec�ficos ao cont�iner.
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

// Adiciona os reposit�rios espec�ficos ao cont�iner.
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// Adiciona o suporte ao NewtonsoftJson aos controllers.
builder.Services.AddControllers().AddNewtonsoftJson();

// Configura as op��es de rota para usar URLs e query strings em min�sculo.
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Configura os servi�os relacionados aos controladores.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(AjustaDataHoraLocal));
}).AddNewtonsoftJson(options =>
{
    // Usa a formata��o padr�o (PascalCase) para as propriedades.
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

// Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configura��o do SwaggerGen para gerar a documenta��o da Web API.
builder.Services.AddSwaggerGen(
    c =>
    {
        // Habilita o uso de anota��es (como [SwaggerOperation]) para melhorar a documenta��o.
        c.EnableAnnotations();
        // Define a vers�o da documenta��o Swagger como "v1".
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

// Configura a pipeline de solicita��o HTTP.
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

// Adiciona o middleware de autoriza��o � pipeline de solicita��o HTTP.
// Este middleware � respons�vel por garantir que o usu�rio esteja autorizado a acessar os recursos solicitados.
app.UseAuthorization();

// Adiciona o middleware de roteamento de controladores � pipeline de solicita��o HTTP.
// Este middleware � respons�vel por rotear as solicita��es HTTP para os controladores apropriados.
app.MapControllers();

// Inicia a execu��o da aplica��o.
// Este m�todo bloqueia o thread chamado e aguarda at� que a aplica��o seja encerrada.
app.Run();
