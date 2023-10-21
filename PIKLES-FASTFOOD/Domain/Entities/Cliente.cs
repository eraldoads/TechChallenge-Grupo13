using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [SwaggerSchemaFilter(typeof(ClienteSchemaFilter))]
    public class Cliente //: IValidatableObject
    {

        [JsonProperty("idCliente")]
        public int IdCliente { get; set; }

        [JsonProperty("nome")]
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ú]+", ErrorMessage = "O nome deve conter apenas letras e somente o primeiro nome")]
        public string? Nome { get; set; }

        [JsonProperty("sobrenome")]
        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [StringLength(50, ErrorMessage = "O sobrenome deve ter no máximo 50 caracteres")]
        public string? Sobrenome { get; set; }

        [JsonProperty("cpf")]
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [CPF(ErrorMessage = "O CPF informado é inválido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "O CPF deve conter somente números")]
        public string? CPF { get; set; }

        [Required]
        [JsonProperty("email")]
        [EmailAddress(ErrorMessage = "O e-mail informado é inválido")]
        public string? Email { get; set; }

        public bool IsValid()
        {
            var validationContext = new ValidationContext(this);
            var validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(this, validationContext, validationResults, true);
        }

        //// Define um método virtual que valida o contexto de validação de outra forma que um DataAnnotation não atenda.
        //public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    // Se o CPF não for válido, retorna um resultado de validação com uma mensagem de erro e o nome do campo.
        //    if (CPF.ToString() == "")
        //        yield return new ValidationResult("CPF Inválido", new string[] { "Nome" });
        //}

    }
}
