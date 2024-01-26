using Domain.Entities;
using Domain.EntitiesDTO;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class ProdutoSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // verifica se o contexto é da classe Produto
            if (context.Type == typeof(Produto))
            {
                // cria um objeto OpenApiObject com os valores desejados
                var modeloProduto = new OpenApiObject
                {
                    ["idProduto"] = new OpenApiInteger(0),
                    ["nomeProduto"] = new OpenApiString("string"),
                    ["valorProduto"] = new OpenApiDouble(0.00),
                    ["IdCategoria"] = new OpenApiInteger(1),
                    ["descricaoProduto"] = new OpenApiString("string"),
                    ["ImagemProduto"] = new OpenApiString("string")
                };
                // atribui o exemplo ao esquema
                schema.Example = modeloProduto;
            }

            // verifica se o contexto é da classe ProdutoDTO
            if (context.Type == typeof(ProdutoDTO))
            {
                // cria um objeto OpenApiObject com os valores desejados
                var modeloProduto = new OpenApiObject
                {
                    ["nomeProduto"] = new OpenApiString("string"),
                    ["valorProduto"] = new OpenApiDouble(0.00),
                    ["IdCategoria"] = new OpenApiInteger(1),
                    ["descricaoProduto"] = new OpenApiString("string"),
                    ["ImagemProduto"] = new OpenApiString("string")
                };
                // atribui o exemplo ao esquema
                schema.Example = modeloProduto;
            }
        }
    }
}
