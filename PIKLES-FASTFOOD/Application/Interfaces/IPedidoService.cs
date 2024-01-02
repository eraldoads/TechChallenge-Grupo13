using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.EntitiesDTO;

namespace Application.Interfaces
{
    public interface IPedidoService
    {
        Task<List<PedidoOutput>> GetPedidos();
        Task<PedidoDTO> PostPedido(PedidoInput pedidoInput);
        Task<bool> UpdateStatusPedido(int idPedido, string novoStatus);
    }
}
