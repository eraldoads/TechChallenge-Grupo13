using Domain.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class ProdutoSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // verifica se o contexto é da classe Pessoa
            if (context.Type == typeof(Produto))
            {
                // cria um objeto OpenApiObject com os valores desejados
                var modeloProduto = new OpenApiObject
                {
                    ["codigoProduto"] = new OpenApiInteger(1300),
                    ["nomeProduto"] = new OpenApiString("Nome do produto - [string]"),
                    ["valorProduto"] = new OpenApiFloat(00),
                    ["idCategoriaProduto"] = new OpenApiInteger(1),
                    ["descricaoProduto"] = new OpenApiString("Descrição do produto - [string]")
                };
                // atribui o exemplo ao esquema
                schema.Example = modeloProduto;
            }
        }
    }
}
