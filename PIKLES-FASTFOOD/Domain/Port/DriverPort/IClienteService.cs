using Domain.Entities;
using Domain.EntitiesDTO;
using Microsoft.AspNetCore.JsonPatch;

namespace Domain.Port.Services
{
    public interface IClienteService
    {
        Task<List<Cliente>> GetClientes();
        Task<Cliente> GetClienteById(int? id);
        Task<Cliente> GetClienteByCpf(string cpf);
        Task<Cliente> PostCliente(ClienteDTO clienteDTO);
        Task PatchCliente(int idCliente, JsonPatchDocument<Cliente> patchDoc);
        Task PutCliente(int idCliente, Cliente clienteInput);
        Task<int> DeleteCliente(int id);
    }
}
