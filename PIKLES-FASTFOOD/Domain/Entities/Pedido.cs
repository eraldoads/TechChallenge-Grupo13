using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

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

        [JsonProperty("valorTotal")]
        public float ValorTotal { get; set; }

    }


}
