using Data.Context;
using Domain.Entities;
using Domain.Port.DrivenPort;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly MySQLContext _context;

        /// <summary>
        /// Inicializa uma nova instância do repositório de clientes com o contexto fornecido.
        /// </summary>
        /// <param name="context">O contexto MySQL para o repositório de clientes.</param>
        public ClienteRepository(MySQLContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os clientes do contexto do banco de dados.
        /// </summary>
        /// <returns>Uma lista de todos os clientes no contexto.</returns>
        public async Task<List<Cliente>> GetClientes()
        {
            if (_context.Cliente is not null)
                return await _context.Cliente.ToListAsync();

            return new List<Cliente>();
        }

        /// <summary>
        /// Obtém um cliente pelo ID no contexto do banco de dados.
        /// </summary>
        /// <param name="id">O ID do cliente a ser recuperado.</param>
        /// <returns>O cliente correspondente ao ID fornecido.</returns>
        public async Task<Cliente?> GetClienteById(int id)
        {
            if (_context.Cliente is null)
                throw new InvalidOperationException("Contexto de produto nulo.");

            var cliente = await _context.Cliente.FindAsync(id);

            return cliente;
        }

        /// <summary>
        /// Obtém um cliente pelo CPF no contexto do banco de dados.
        /// </summary>
        /// <param name="cpf">O CPF do cliente a ser recuperado.</param>
        /// <returns>O cliente correspondente ao CPF fornecido.</returns>
        public async Task<Cliente?> GetClienteByCpf(string cpf)
        {
            if (_context is null || _context.Cliente is null)
            {
                return null;
            }

            return await _context.Cliente.FirstOrDefaultAsync(c => c.CPF == cpf);
        }

        /// <summary>
        /// Adiciona um novo cliente ao contexto do banco de dados.
        /// </summary>
        /// <param name="cliente">O cliente a ser adicionado ao contexto.</param>
        /// <returns>O cliente recém-adicionado.</returns>
        public async Task<Cliente> PostCliente(Cliente cliente)
        {
            if (_context.Cliente is not null)
            {
                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();
            }

            return cliente;
        }

        /// <summary>
        /// Atualiza um cliente existente no contexto do banco de dados.
        /// </summary>
        /// <param name="cliente">O cliente a ser atualizado no contexto.</param>
        /// <returns>O número de entradas modificadas no contexto do banco de dados.</returns>
        public async Task<int> UpdateCliente(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Substitui um cliente existente no contexto do banco de dados.
        /// </summary>
        /// <param name="cliente">O cliente a ser substituído no contexto.</param>
        /// <returns>O número de entradas modificadas no contexto do banco de dados.</returns>
        public async Task<int> PutCliente(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Exclui um cliente com o ID fornecido do contexto do banco de dados.
        /// </summary>
        /// <param name="id">O ID do cliente a ser excluído.</param>
        /// <returns>O número de entradas modificadas no contexto do banco de dados.</returns>
        public async Task<int> DeleteCliente(int id)
        {
            if (_context.Cliente is null)
                throw new InvalidOperationException("O contexto do cliente é nulo.");

            var cliente = await _context.Cliente.FindAsync(id) ?? throw new KeyNotFoundException($"O cliente com o ID {id} não foi encontrado.");

            _context.Cliente.Remove(cliente);
            return await _context.SaveChangesAsync();
        }
    }
}
