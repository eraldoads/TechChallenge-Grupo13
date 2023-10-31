using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.EntitiesDTO;

namespace Domain.Port.DriverPort
{
    public interface IPedidoService
    {
        Task<List<PedidoOutput>> GetPedidos();
        Task<PedidoDTO> PostPedido(PedidoInput pedidoInput);
    }
}
