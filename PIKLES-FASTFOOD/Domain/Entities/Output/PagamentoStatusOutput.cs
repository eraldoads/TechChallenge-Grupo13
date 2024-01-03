using Newtonsoft.Json;

namespace Domain.Entities.Output
{
    [JsonObject]
    public class PagamentoStatusOutput
    {
        [JsonProperty(Order = int.MaxValue)]
        public string? StatusPagamento { get; set; }
    }
}
