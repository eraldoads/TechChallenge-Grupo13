using Domain.Entities;
using Domain.Entities.Output;

namespace Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<List<PedidoOutput>> GetPedidos();
        Task<Pedido> PostPedido(Pedido pedido);

        Task<bool> ClienteExists(int clienteId);
        Task<bool> ProdutoExists(int produtoId);

        Task<Pedido?> GetPedidoById(int idPedido);
        Task UpdatePedido(Pedido pedido);
    }
}
