using Domain.Entities;
using Domain.Entities.Output;

namespace Domain.Port.DrivenPort
{
    public interface IPedidoRepository
    {
        Task<List<PedidoOutput>> GetPedidos();

        Task PostPedido(Pedido pedido);
    }
}
