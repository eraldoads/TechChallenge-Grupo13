using Domain.ValueObjects;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    //[SwaggerSchemaFilter(typeof(LoginSchemaFilter))]
    public class Login
    {
        [JsonProperty("cpf")]
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [CPF(ErrorMessage = "O CPF informado é inválido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "O CPF deve conter somente números")]
        public string? CPF { get; set; }

    }
}
