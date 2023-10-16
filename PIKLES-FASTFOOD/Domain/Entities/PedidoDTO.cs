using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Domain.Entities
{
    /// <summary>
    /// Transferir dados entre camadas.
    /// </summary>
    [SwaggerSchemaFilter(typeof(PedidoDTOSchemaFilter))]
    public class PedidoDTO
    {
        public string? Cliente { get; set; }
        public DateTimeOffset DataPedido { get; set; }
        public int Quantidade { get; set; }
        public string? NomeCategoria { get; set; }
        public int CodigoProduto { get; set; }
        public string? NomeProduto { get; set; }
        public float ValorProduto { get; set; }

    }
}
