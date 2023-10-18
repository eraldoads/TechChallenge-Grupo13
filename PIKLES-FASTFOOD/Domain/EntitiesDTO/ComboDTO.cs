using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Domain.EntitiesDTO
{
    /// <summary>
    /// Transferir dados entre camadas.
    /// </summary>
    //[SwaggerSchemaFilter(typeof(PedidoDTOSchemaFilter))]
    public class ComboDTO
    {
        public List<ProdutoDTO>? Produto { get; set; }

    }
}
