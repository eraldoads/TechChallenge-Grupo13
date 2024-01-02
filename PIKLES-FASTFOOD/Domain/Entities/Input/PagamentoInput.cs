namespace Domain.Entities.Input
{
    //[SwaggerSchemaFilter(typeof(PagamentoInputSchemaFilter))]
    public class PagamentoInput
    {
        public string? IdTransacao { get; set; } // Identificador único da transação
        public decimal Valor { get; set; } // Valor do pagamento
        public string? MetodoPagamento { get; set; } // Método de pagamento (cartão de crédito, boleto, etc.)
        // Outras propriedades necessárias...

        // Adicione propriedades adicionais conforme necessário para a integração com o Mercado Pago.
    }

}
