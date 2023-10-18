using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Domain.EntitiesDTO
{
    /// <summary>
    /// Transferir dados entre camadas.
    /// </summary>
    //[SwaggerSchemaFilter(typeof(PedidoDTOSchemaFilter))]
    public class ProdutoDTO
    {
        public int IdProduto { get; set; }
        public string? NomeProduto { get; set; }
        public int QuantidadeProduto { get; set; }
        public float ValorProduto { get; set; }

    }
}
