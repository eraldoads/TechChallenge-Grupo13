using Newtonsoft.Json;

namespace Domain.Entities
{
    //[SwaggerSchemaFilter(typeof(ProdutoSchemaFilter))]
    public class Categoria
    {
        [JsonProperty("idCategoria")]
        public int IdCategoria { get; set; }

        [JsonProperty("nomeCategoria")]
        public string? NomeCategoria { get; set; }
    }
}
