using Domain.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class PedidoSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // verifica se o contexto é da classe Pedido
            if (context.Type == typeof(Pedido))
            {
                // cria um objeto OpenApiObject com os valores desejados
                var modeloPedido = new OpenApiObject
                {
                    ["Id"] = new OpenApiInteger(0),
                    ["IdCliente"] = new OpenApiInteger(0),
                    ["DataPedido"] = new OpenApiString(DateTime.Now.ToString("yyyy-MM-dd")),
                    ["ValorTotal"] = new OpenApiInteger(0),
                };
                // atribui o exemplo ao esquema
                schema.Example = modeloPedido;
            }
        }
    }
}
