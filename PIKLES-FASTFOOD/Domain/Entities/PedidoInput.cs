using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [SwaggerSchemaFilter(typeof(PedidoInputSchemaFilter))]
    public class PedidoInput
    {
        [Required(ErrorMessage = "O id do cliente é obrigatório")]
        public int IdCliente { get; set; } // O id do cliente que fez o pedido

        [Required(ErrorMessage = "A data do pedido é obrigatória")]
        [DataType(DataType.Date, ErrorMessage = "A data do pedido deve estar no formato aaaa-mm-dd")]
        public DateTimeOffset DataPedido { get; set; } // A data do pedido

        [Required(ErrorMessage = "A lista de produtos é obrigatória")]
        [MinLength(1, ErrorMessage = "A lista de produtos deve ter pelo menos um item")]
        public List<ProdutoInput>? Produtos { get; set; } // A lista de produtos que compõem o pedido
    }

}
