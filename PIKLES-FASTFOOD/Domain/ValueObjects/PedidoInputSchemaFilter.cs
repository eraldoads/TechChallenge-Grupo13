using Domain.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class PedidoInputSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // verifica se o contexto é da classe PedidoInput
            if (context.Type == typeof(PedidoInput))
            {
                // cria um objeto OpenApiObject para a propriedade IdCliente
                var modeloIdCliente = new OpenApiObject
                {
                    ["IdCliente"] = new OpenApiInteger(1),
                };

                // cria um objeto OpenApiObject para a propriedade DataPedido
                var modeloDataPedido = new OpenApiObject
                {
                    ["DataPedido"] = new OpenApiString(DateTime.Now.ToString("yyyy-MM-dd")),
                };

                // cria um objeto OpenApiObject para a propriedade Produtos
                var modeloProdutos = new OpenApiArray
                {
                    // adiciona dois objetos OpenApiObject ao array, representando dois produtos
                    new OpenApiObject
                    {
                        ["IdProduto"] = new OpenApiInteger(1),
                        ["Quantidade"] = new OpenApiInteger(2),
                    },
                    new OpenApiObject
                    {
                        ["IdProduto"] = new OpenApiInteger(3),
                        ["Quantidade"] = new OpenApiInteger(1),
                    }
                };

                // cria um objeto OpenApiObject com os valores desejados
                var modeloProduto = new OpenApiObject
                {
                    // adiciona os objetos criados anteriormente ao modeloProduto, usando os nomes das propriedades como chaves
                    { "IdCliente", modeloIdCliente["IdCliente"] },
                    { "DataPedido", modeloDataPedido["DataPedido"] },
                    { "Produtos", modeloProdutos }
                };

                // atribui o exemplo ao esquema
                schema.Example = modeloProduto;
            }
        }
    }
}
