using Newtonsoft.Json;

namespace Domain.Entities
{
    //[SwaggerSchemaFilter(typeof(ProdutoSchemaFilter))]
    public class Pedido
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("idCliente")]
        public int IdCliente { get; set; }

        [JsonProperty("dataPedido")]
        public DateTime DataPedido { get; set; }

        [JsonProperty("valorTotal")]
        public float ValorTotal { get; set; }
    }
}
