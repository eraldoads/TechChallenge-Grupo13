using Domain.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace Domain.Entities.Output
{
    [SwaggerSchemaFilter(typeof(PedidoOutputDTOSchemaFilter))]
    public class PedidoOutput
    {
        public int IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public string? StatusPedido { get; set; }
        public string? NomeCliente { get; set; }
        public float ValorTotalPedido { get; set; }
        public List<ComboOutput>? Combo { get; set; }
    }
}
