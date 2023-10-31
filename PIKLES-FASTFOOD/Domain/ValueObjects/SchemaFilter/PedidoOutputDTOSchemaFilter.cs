using Domain.Entities.Output;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class PedidoOutputDTOSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(PedidoOutput))
            {
                var modeloIdPedido = new OpenApiObject
                {
                    ["idPedido"] = new OpenApiInteger(1),
                };

                var modeloDataPedido = new OpenApiObject
                {
                    ["dataPedido"] = new OpenApiString(DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                };

                var modeloStatusPedido = new OpenApiObject
                {
                    ["statusPedido"] = new OpenApiString("string"),
                };

                var modeloNomeCliente = new OpenApiObject
                {
                    ["nomeCliente"] = new OpenApiString("string"),
                };

                var modeloValorTotalPedido = new OpenApiObject
                {
                    ["valorTotalPedido"] = new OpenApiDouble(0.00),
                };

                var modeloProdutos = new OpenApiArray
                {
                    new OpenApiObject
                    {
                        ["idCombo"] = new OpenApiInteger(1),
                        ["produto"] = new OpenApiArray
                        {
                            new OpenApiObject
                            {
                                ["idProduto"] = new OpenApiInteger(1),
                                ["nomeProduto"] = new OpenApiString("string"),
                                ["quantidadeProduto"] = new OpenApiInteger(1),
                                ["valorProduto"] = new OpenApiDouble(0.00),
                            }
                        }
                    }
                };

                var modeloPedido = new OpenApiObject
                {
                    { "idPedido", modeloIdPedido["idPedido"] },
                    { "dataPedido", modeloDataPedido["dataPedido"] },
                    { "statusPedido", modeloStatusPedido["statusPedido"] },
                    { "nomeCliente", modeloNomeCliente["nomeCliente"] },
                    { "valorTotalPedido", modeloValorTotalPedido["valorTotalPedido"] },
                    { "combo", modeloProdutos }
                };

                schema.Example = modeloPedido;
            }
        }
    }
}
