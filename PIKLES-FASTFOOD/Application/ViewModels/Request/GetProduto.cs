using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace App.Application.ViewModels.Request
{
    public class GetProduto : IValidatableObject
    {
        [FromQuery]
        [JsonProperty("nomeProduto")]
        public string? NomeProduto { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(NomeProduto))
                yield return new ValidationResult("O parâmetro NomeProduto é obrigatório e deve ter no minimo 20 caracteres", new string[] { "NomeProduto" });
        }
    }
}
