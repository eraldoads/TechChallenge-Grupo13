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
            // verifica se o contexto é da classe PedidoDTO
            if (context.Type == typeof(PedidoDTO))
            {
                // cria um objeto OpenApiObject com os valores desejados
                var modeloPedidoDTO = new OpenApiObject
                {
                    ["Cliente"] = new OpenApiString("[string]"),
                    ["DataPedido"] = new OpenApiString(DateTime.Now.ToString("yyyy-MM-dd")),
                    ["Quantidade"] = new OpenApiInteger(0),
                    ["NomeCategoria"] = new OpenApiString("[string]"),
                    ["CodigoProduto"] = new OpenApiInteger(0),
                    ["NomeProduto"] = new OpenApiString("[string]"),
                    ["ValorProduto"] = new OpenApiDouble(0.01)
                };
                // atribui o exemplo ao esquema
                schema.Example = modeloPedidoDTO;
            }
        }
    }
}
