namespace Domain.EntitiesDTO
{
    /// <summary>
    /// Transferir dados entre camadas.
    /// </summary>
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public float ValorTotal { get; set; }
    }
}
