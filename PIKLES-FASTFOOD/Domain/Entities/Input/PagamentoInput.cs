namespace Domain.Entities.Input
{
    //[SwaggerSchemaFilter(typeof(PagamentoInputSchemaFilter))]
    public class PagamentoInput
    {
        public string? Id { get; set; } // Identificador único da transação
        public decimal Valor { get; set; } // Valor do pagamento
        public string? MetodoPagamento { get; set; } = "QRCode"; // Método de pagamento (cartão de crédito, boleto, etc.)
        // Outras propriedades necessárias...

        // Adicione propriedades adicionais conforme necessário para a integração com o Mercado Pago.


        /* 
         DATA DO PAGAMENTO
         ID PEDIDO         
         IDTRANSACAO
            "data": {
                "id": "1320419585"
              },


        {
          "action": "payment.created",
          "api_version": "v1",
          "data": {
            "id": "1320419585"
          },
          "date_created": "2024-01-02T20:21:50Z",
          "id": 109574408792,
          "live_mode": false,
          "type": "payment",
          "user_id": "1619908702"
        }
          
         */
    }

}
