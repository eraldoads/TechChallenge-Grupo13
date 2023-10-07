using Domain.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class ClienteSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // Verifica se o contexto é da classe Pessoa.
            if (context.Type == typeof(Cliente))
            {
                // Cria um objeto OpenApiObject com os valores desejados.
                var modeloCliente = new OpenApiObject
                {
                    ["nome"] = new OpenApiString("Nome do Cliente - [string]"),
                    ["sobrenome"] = new OpenApiString("Sobrenome do Cliente - [string]"),
                    ["CPF"] = new OpenApiString("CPF válido do cliente - [xxx.xxx.xxx-xx]"),
                    ["email"] = new OpenApiString("email válido do cliente - [exemplo@email.com.br]")
                };
                // Atribui o exemplo ao esquema.
                schema.Example = modeloCliente;
            }
        }
    }
}
