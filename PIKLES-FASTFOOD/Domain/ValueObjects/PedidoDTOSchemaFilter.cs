using Domain.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class PedidoDTOSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // verifica se o contexto é da classe Pessoa
            if (context.Type == typeof(PedidoDTO))
            {
                // cria um objeto OpenApiObject com os valores desejados
                var modeloProduto = new OpenApiObject
                {
                    ["Cliente"] = new OpenApiString("Nome completo do cliente - [string]"),
                    ["Quantidade"] = new OpenApiInteger(0),
                    ["NomeCategoria"] = new OpenApiString("Nome da categoria do produto - [string]"),
                    ["CodigoProduto"] = new OpenApiInteger(0),
                    ["NomeProduto"] = new OpenApiString("Nome do produto - [string]"),
                    ["ValorProduto"] = new OpenApiDouble(0.01)
                };
                // atribui o exemplo ao esquema
                schema.Example = modeloProduto;
            }
        }
    }
}
