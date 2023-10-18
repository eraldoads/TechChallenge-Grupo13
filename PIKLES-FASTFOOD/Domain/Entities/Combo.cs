using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    //[SwaggerSchemaFilter(typeof(LoginSchemaFilter))]
    public class Combo
    {
        [JsonProperty("produto")]
        public List<ProdutoInput>? Produto { get; set; }
    }
}
