using Domain.Entities.Input;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class PedidoInputSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(PedidoInput))
            {
                var modeloIdCliente = new OpenApiObject
                {
                    ["idCliente"] = new OpenApiInteger(1),
                };

                var modeloDataPedido = new OpenApiObject
                {
                    ["dataPedido"] = new OpenApiString(DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                };

                var modeloStatusPedido = new OpenApiObject
                {
                    ["statusPedido"] = new OpenApiString("Status do pedido"),
                };

                var modeloProdutos = new OpenApiArray
                {
                    new OpenApiObject
                    {
                        ["produto"] = new OpenApiArray
                        {
                            new OpenApiObject
                            {
                                ["idProduto"] = new OpenApiInteger(2),
                                ["quantidade"] = new OpenApiInteger(3),
                            }
                        }
                    },
                    new OpenApiObject
                    {
                        ["produto"] = new OpenApiArray
                        {
                            new OpenApiObject
                            {
                                ["idProduto"] = new OpenApiInteger(4),
                                ["quantidade"] = new OpenApiInteger(5),
                            }
                        }
                    }
                };

                var modeloPedido = new OpenApiObject
                {
                    { "idCliente", modeloIdCliente["idCliente"] },
                    { "dataPedido", modeloDataPedido["dataPedido"] },
                    { "statusPedido", modeloStatusPedido["statusPedido"] },
                    { "combo", modeloProdutos }
                };

                schema.Example = modeloPedido;
            }
        }
    }
}
