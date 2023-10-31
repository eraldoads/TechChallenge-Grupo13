using Domain.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.ValueObjects
{
    public class LoginSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // Verifica se o contexto é da classe Login.
            if (context.Type == typeof(Login))
            {
                // Cria um objeto OpenApiObject com os valores desejados.
                var modeloLogin = new OpenApiObject
                {
                    ["cpf"] = new OpenApiString("string")
                };
                // Atribui o exemplo ao esquema.
                schema.Example = modeloLogin;
            }
        }
    }
}
