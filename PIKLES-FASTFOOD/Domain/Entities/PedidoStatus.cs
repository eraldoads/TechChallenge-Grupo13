using Domain.Base;
using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Domain.Entities
{
    [SwaggerSchemaFilter(typeof(PedidoSchemaFilter))]
    public class PedidoStatus
    {
        [JsonProperty("statusPedido")]
        public EnumStatusPedido? StatusPedido { get; set; }
    }
}
