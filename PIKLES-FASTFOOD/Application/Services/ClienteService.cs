using Domain.Adapters;
using Domain.Entities;
using Domain.Services;

namespace Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente> GetClienteById(int? id)
        {
            return await _clienteRepository.GetClienteById(id.Value);
        }

        public async Task<Cliente> GetClienteByCpf(string cpf)
        {
            return await _clienteRepository.GetClienteByCpf(cpf);
        }

        public async Task<List<Cliente>> GetClientes()
        {
            return await _clienteRepository.GetClientes();
        }

        public async Task<Cliente> PostCliente(Cliente cliente)
        {
            return await _clienteRepository.PostCliente(cliente);
        }

        public async Task<int> UpdateCliente(Cliente cliente)
        {
            return await _clienteRepository.UpdateCliente(cliente);
        }

        public async Task<int> DeleteCliente(int id)
        {
            return await _clienteRepository.DeleteCliente(id);
        }
    }
}
