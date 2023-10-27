using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Input
{
    [SwaggerSchemaFilter(typeof(PedidoInputSchemaFilter))]
    public class PedidoInput
    {
        [Required(ErrorMessage = "O id do cliente é obrigatório")]
        public int IdCliente { get; set; } // O id do cliente que fez o pedido

        [Required(ErrorMessage = "A data do pedido é obrigatória")]
        [DataType(DataType.DateTime, ErrorMessage = "A data do pedido deve estar no formato aaaa-mm-dd HH:mm")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime DataPedido { get; set; } // A data do pedido

        [Required(ErrorMessage = "O status do pedido é obrigatório")]
        public string? StatusPedido { get; set; } // O status do pedido

        [Required(ErrorMessage = "A lista de combos é obrigatória")]
        [MinLength(1, ErrorMessage = "A lista de combos deve ter pelo menos um item")]
        public List<ComboInput>? Combo { get; set; } // A lista de combos que compõem o pedido
    }

}
