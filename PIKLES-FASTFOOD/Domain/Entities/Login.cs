using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [SwaggerSchemaFilter(typeof(LoginSchemaFilter))]
    public class Login //: IValidatableObject
    {
        [JsonProperty("cpf")]
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [CPF(ErrorMessage = "O CPF informado é inválido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "O CPF deve conter somente números")]
        public string? CPF { get; set; }

        //// Define um método virtual que valida o contexto de validação de outra forma que um DataAnnotation não atenda.
        //public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    // Se o CPF não for válido, retorna um resultado de validação com uma mensagem de erro e o nome do campo.
        //    if (string.IsNullOrEmpty(CPF))
        //        yield return new ValidationResult("O CPF é obrigatório", new string[] { "CPF" });
        //}
    }
}
