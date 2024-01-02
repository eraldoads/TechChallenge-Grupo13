namespace Domain.Entities.Output
{
    /// <summary>
    /// Transferir dados entre camadas.
    /// </summary>
    //[SwaggerSchemaFilter(typeof(PagamentoSchemaFilter))]
    public class PagamentoOutput
    {
        public int IdPagamento { get; set; }
        public decimal Valor { get; set; }
        public string MetodoPagamento { get; set; }
    }
}
