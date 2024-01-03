using Data.Context;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly MySQLContext _context;

        public PagamentoRepository(MySQLContext context)
        {
            _context = context;
        }

        public async Task<Pagamento?> GetPagamentoByIdPedido(int idPedido)
        {
            if (_context.Pagamento is null)
                throw new InvalidOperationException("Contexto de pagamento nulo.");

            // Caso encontre mais de um registro pega sempre o ultimo.
            return await _context.Pagamento.Where(p => p.IdPedido == idPedido)
                                           .OrderByDescending(p => p.DataPagamento)
                                           .FirstOrDefaultAsync();
        }

        public async Task<Pagamento> PostPagamento(Pagamento pagamento)
        {
            if (_context.Pagamento is not null)
            {
                _context.Pagamento.Add(pagamento);
                await _context.SaveChangesAsync();
            }
            return pagamento;
        }
    }
}
