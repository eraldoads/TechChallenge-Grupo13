using Domain.Entities;
using Domain.Entities.Output;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPagamentoService
    {
        Task<PagamentoOutput> ProcessarPagamento(Pagamento pagamentoInput);
        Task<PagamentoStatusOutput?> GetStatusPagamento(int idPedido);
        Task<QRCodeOutput?> CriarQRCodePagamento(int idPedido);
    }
}
