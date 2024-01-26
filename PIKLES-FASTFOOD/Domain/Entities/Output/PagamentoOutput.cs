using Newtonsoft.Json;

namespace Domain.Entities.Output
{
    [JsonObject]
    public class PagamentoOutput : PagamentoStatusOutput
    {
        [JsonProperty(Order = 1)]
        public int IdPagamento { get; set; }
        [JsonProperty(Order = 2)]
        public float ValorPagamento { get; set; }
        [JsonProperty(Order = 3)]
        public string? MetodoPagamento { get; set; }
        [JsonProperty(Order = 4)]
        public int IdPedido { get; set; }
    }
}
