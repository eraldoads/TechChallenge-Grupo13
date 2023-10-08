using Newtonsoft.Json;

namespace Domain.Entities
{
    //[SwaggerSchemaFilter(typeof(ProdutoSchemaFilter))]
    public class Categoria
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nomeCategoria")]
        public string? NomeCategoria { get; set; }

    }
}
