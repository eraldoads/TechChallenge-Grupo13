using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PedidoInput
    {
        [Required(ErrorMessage = "O id do cliente é obrigatório")]
        public int IdCliente { get; set; } // O id do cliente que fez o pedido

        [Required(ErrorMessage = "A data do pedido é obrigatória")]
        [DataType(DataType.Date, ErrorMessage = "A data do pedido deve estar no formato dd/mm/aaaa")]
        public DateTime DataPedido { get; set; } // A data do pedido

        [Required(ErrorMessage = "A lista de produtos é obrigatória")]
        [MinLength(1, ErrorMessage = "A lista de produtos deve ter pelo menos um item")]
        [MaxLength(10, ErrorMessage = "A lista de produtos deve ter no máximo dez itens")]
        public List<ProdutoInput> Produtos { get; set; } // A lista de produtos que compõem o pedido
    }

}
