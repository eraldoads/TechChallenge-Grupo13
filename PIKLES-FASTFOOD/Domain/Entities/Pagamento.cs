using Domain.ValueObjects;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [SwaggerSchemaFilter(typeof(PagamentoInputSchemaFilter))]
    public class Pagamento
    {
        [JsonProperty("idPagamento")]
        public int IdPagamento { get; set; } // Identificador único da transação

        [JsonProperty("statusPagamento")]
        public string? StatusPagamento { get; set; } = "Pendente"; // TODO: Transformar em um ENUM

        [JsonProperty("valorPagamento")]
        public float ValorPagamento { get; set; } // Valor do pagamento

        [JsonProperty("metodoPagamento")]
        public string? MetodoPagamento { get; set; } = "QRCode"; // Método de pagamento (QRCode, cartão de crédito, boleto, etc.)

        [Required(ErrorMessage = "A data do pedido é obrigatória")]
        [DataType(DataType.DateTime, ErrorMessage = "A data do pagamento deve estar no formato aaaa-mm-dd HH:mm")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime DataPagamento { get; set; } // A data do pedido

        [JsonProperty("idPedido")]
        [ForeignKey("IdPedido")]
        public int IdPedido { get; set; } // Id do Pedido pago.

        [JsonIgnore]
        public Pedido? Pedido { get; set; } // Propriedade de navegação para o objeto Pedido
    }

}
