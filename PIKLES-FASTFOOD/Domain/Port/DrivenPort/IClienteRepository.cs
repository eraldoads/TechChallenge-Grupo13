using Domain.Entities;

namespace Domain.Port.Adapters
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetClientes();
        Task<Cliente> GetClienteById(int id);
        Task<Cliente> GetClienteByCpf(string cpf);
        Task<Cliente> PostCliente(Cliente cliente);
        Task<int> UpdateCliente(Cliente cliente);
        Task<int> DeleteCliente(int id);
    }
}
