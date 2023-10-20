using Domain.Entities;
using Domain.EntitiesDTO;

namespace Domain.Services
{
    public interface IClienteService
    {
        Task<List<Cliente>> GetClientes();
        Task<Cliente> GetClienteById(int? id);
        Task<Cliente> GetClienteByCpf(string cpf);
        Task<Cliente> PostCliente(ClienteDTO clienteDTO);
        Task UpdateCliente(Cliente cliente);
        Task<int> DeleteCliente(int id);
    }
}
