using Newtonsoft.Json;

namespace App.Application.ViewModels.Response
{
    public class Produto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("codigoProduto")]
        public int CodigoProduto { get; set; }

        [JsonProperty("nomeProduto")]
        public string? NomeProduto { get; set; }

        [JsonProperty("valorProduto")]
        public float ValorProduto { get; set; }

        [JsonProperty("categoriaProduto")]
        public string? Categoria { get; set; }

        [JsonProperty("descricaoProduto")]
        public string? Descricao { get; set; }
    }
}
