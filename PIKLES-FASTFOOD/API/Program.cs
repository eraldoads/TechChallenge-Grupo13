using Application.Interfaces;
using Data.Context;
using Data.Repository;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Globalization;

// Cria um builder de aplicação web com os argumentos passados.
var builder = WebApplication.CreateBuilder(args);

// Adiciona serviÇos ao contêiner.

// Cria uma variável que armazena a string de conexão com o banco de dados MySQL.
var connectionStringMysql = builder.Configuration.GetConnectionString("ConnectionMysql");

// Adiciona um serviço do tipo MySQLContext ao objeto builder.Services.
builder.Services.AddDbContext<MySQLContext>(option => option.UseMySql(
    connectionStringMysql, // Usa a string de conexão.
    ServerVersion.AutoDetect(connectionStringMysql), // Especifica a versão do servidor MySQL.
    builder => builder.MigrationsAssembly("API") // Especifica o assembly do projeto que contém as classes de migrações do EF Core.
));

// Adiciona os serviÇos de controllers ao builder.
builder.Services.AddControllers(options =>
{
    // Insere um formato de entrada personalizado para o JsonPatch.
    options.InputFormatters.Insert(0, JsonPatchSample.MyJPIF.GetJsonPatchInputFormatter());
});

// Adiciona os serviÇos específicos ao contêiner.
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

// Configura os serviÇos relacionados aos controladores.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(AjustaDataHoraLocal));
}).AddNewtonsoftJson(options =>
{
    // Usa a formatação padrão (PascalCase) para as propriedades.
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    options.SerializerSettings.Formatting = Formatting.Indented;
    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
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
            Description = "Documentação dos endpoints da API.",
            Contact = new OpenApiContact() { Name = "Tech Challenge - Grupo 13", Email = "grupo13@fiap.com" },
            License = new OpenApiLicense() { Name = "MIT License", Url = new Uri("https://opensource.org/licenses/MIT") },
            Version = "1.0.11"
        });

        // Habilita o uso para registrar o SchemaFilter.
        c.SchemaFilter<ClienteSchemaFilter>();
    }
);

var app = builder.Build();

// Adiciona o middleware de codificação para garantir a codificação correta em todas as respostas.
app.Use((context, next) =>
{
    context.Response.Headers["Content-Type"] = "application/json; charset=utf-8";
    context.Response.Headers["Content-Encoding"] = "utf-8";
    context.Response.Headers["Content-Language"] = CultureInfo.CurrentCulture.Name;
    return next();
});

// Configura a pipeline de solicitação HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.DefaultModelRendering(ModelRendering.Example);
        /// c.DefaultModelExpandDepth(-1);
        /// c.DefaultModelsExpandDepth(-1);
        /// c.DocExpansion(DocExpansion.None);
        /// c.DisplayRequestDuration();
        c.DisplayOperationId();
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
        c.EnableValidator();
        /// c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete);
    });

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
