using Domain.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class ProdutoImputSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // verifica se o contexto é da classe Pessoa
            if (context.Type == typeof(ProdutoInput))
            {
                // cria um objeto OpenApiObject com os valores desejados
                var modeloProduto = new OpenApiObject
                {
                    ["IdProduto"] = new OpenApiInteger(0),
                    ["Quantidade"] = new OpenApiInteger(0),
                };
                // atribui o exemplo ao esquema
                schema.Example = modeloProduto;
            }
        }
    }
}
