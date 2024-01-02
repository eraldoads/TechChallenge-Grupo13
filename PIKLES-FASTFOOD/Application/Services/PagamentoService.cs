using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PagamentoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PagamentoOutput> ProcessarPagamento(PagamentoInput pagamentoInput)
        {
            // Lógica de processamento...

            // Retorne o resultado, pode ser um PagamentoOutput ou outro tipo adequado.
            //return resultadoDoProcessamento;

            return null;
        }

    }
}
