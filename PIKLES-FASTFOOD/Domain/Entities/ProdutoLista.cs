using Newtonsoft.Json;

namespace Domain.Entities
{
    public class ProdutoLista
    {
        [JsonProperty("idProduto")]
        public int IdProduto { get; set; }

        [JsonProperty("nomeProduto")]
        public string? NomeProduto { get; set; }

        [JsonProperty("valorProduto")]
        public float ValorProduto { get; set; }

        [JsonProperty("idCategoria")]
        public int IdCategoria { get; set; }

        [JsonProperty("nomeCategoria")]
        public string? NomeCategoria { get; set; }

        [JsonProperty("descricaoProduto")]
        public string? DescricaoProduto { get; set; }

        [JsonProperty("imagemProduto")]
        public string? ImagemProduto { get; set; }
    }
}
