using Domain.Entities;
using Domain.Entities.Output;

namespace Application.Interfaces
{
    public interface IPagamentoService
    {
        Task<PagamentoOutput> ProcessarPagamento(Pagamento pagamentoInput);
        Task<PagamentoStatusOutput?> GetStatusPagamento(int idPedido);
    }
}
