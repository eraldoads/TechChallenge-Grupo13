using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [SwaggerSchemaFilter(typeof(PedidoSchemaFilter))]
    public class Pedido
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("idCliente")]
        public int IdCliente { get; set; }

        [JsonProperty("dataPedido")]
        public DateTimeOffset DataPedido { get; set; }

        [Required(ErrorMessage = "O status do pedido é obrigatório")]
        public string? StatusPedido { get; set; }

        [JsonProperty("valorTotal")]
        public float ValorTotal { get; set; }

    }
}
