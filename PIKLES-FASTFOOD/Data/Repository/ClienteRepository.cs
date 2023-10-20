using Data.Context;
using Domain.Adapters;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly MySQLContext _context;

        public ClienteRepository(MySQLContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> GetClientes()
        {
            return await _context.Cliente.ToListAsync();
        }

        public async Task<Cliente> GetClienteById(int id)
        {
            return await _context.Cliente.FindAsync(id);
        }

        public async Task<Cliente> GetClienteByCpf(string cpf)
        {
            return await _context.Cliente.FirstOrDefaultAsync(c => c.CPF == cpf);
        }

        public async Task<Cliente> PostCliente(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            _context.SaveChanges();

            return cliente;
        }

        public async Task<int> UpdateCliente(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            _context.Cliente.Remove(cliente);

            return await _context.SaveChangesAsync();
        }
    }
}
