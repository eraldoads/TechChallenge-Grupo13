using Newtonsoft.Json;

namespace App.Application.ViewModels.Response
{
    public class Produto
    {
        [JsonProperty("codigoProduto")]
        public int CodigoProduto { get; set; }
        
        [JsonProperty("nomeProduto")]
        public string? NomeProduto { get; set; }

        [JsonProperty("valorProduto")]
        public float ValorProduto { get; set; }
    }
}
