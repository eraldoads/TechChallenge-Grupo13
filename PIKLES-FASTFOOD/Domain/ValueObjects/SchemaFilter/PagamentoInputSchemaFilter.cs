using Domain.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class PagamentoInputSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // verifica se o contexto é da classe Pagamento
            if (context.Type == typeof(Pagamento))
            {
                // cria um objeto OpenApiObject com os valores desejados
                var modeloPagamento = new OpenApiObject
                {
                    ["StatusPagamento"] = new OpenApiString("Pendente"),
                    ["ValorPagamento"] = new OpenApiInteger(0),
                    ["MetodoPagamento"] = new OpenApiString("string"),
                    ["DataPagamento"] = new OpenApiString(DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                    ["IdPedido"] = new OpenApiInteger(0),
                };
                // atribui o exemplo ao esquema
                schema.Example = modeloPagamento;
            }
        }
    }
}
