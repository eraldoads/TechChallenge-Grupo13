using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    //[SwaggerSchemaFilter(typeof(PedidoSchemaFilter))]
    public class Pedido
    {
        [JsonProperty("idPedido")]
        public int IdPedido { get; set; }

        [JsonProperty("idCliente")]
        public int IdCliente { get; set; }

        [JsonProperty("dataPedido")]
        public DateTime DataPedido { get; set; }

        [Required(ErrorMessage = "O status do pedido é obrigatório")]
        public string? StatusPedido { get; set; }

        [JsonProperty("valorTotal")]
        public float ValorTotal { get; set; }

        public List<Combo> Combos { get; set; } = new List<Combo>();
    }
}
