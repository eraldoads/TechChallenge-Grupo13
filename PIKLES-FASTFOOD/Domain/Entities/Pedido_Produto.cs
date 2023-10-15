using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    //[SwaggerSchemaFilter(typeof(ProdutoSchemaFilter))]
    public class Pedido_Produto
    {
        [Key]
        [Column(Order = 0)]
        [JsonProperty("idPedido")]
        public int IdPedido { get; set; }

        [Key]
        [Column(Order = 1)]
        [JsonProperty("idProduto")]
        public int IdProduto { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }
    }
}