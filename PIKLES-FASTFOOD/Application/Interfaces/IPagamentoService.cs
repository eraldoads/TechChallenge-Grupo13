using Domain.Entities.Input;
using Domain.Entities.Output;

namespace Application.Interfaces
{
    public interface IPagamentoService
    {
        Task<PagamentoOutput> ProcessarPagamento(PagamentoInput pagamentoInput);
    }
}
