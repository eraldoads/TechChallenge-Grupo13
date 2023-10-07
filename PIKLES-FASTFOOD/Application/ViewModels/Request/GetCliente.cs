using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace App.Application.ViewModels.Request
{
    public class GetCliente //: IValidatableObject
    {
        [FromQuery]
        [Required]
        [JsonProperty("cpf")]
        [CPF(ErrorMessage = "O CPF informado é inválido.")]
        public string? CPF { get; set; }

        //public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (!CPFIsValid(CPF))
        //        yield return new ValidationResult("CPF Inválido", new string[] { "Nome" });
        //}

    }
}