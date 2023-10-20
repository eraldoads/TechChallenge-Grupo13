using Domain.Adapters;
using Domain.Entities;
using Domain.EntitiesDTO;
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

        public async Task<Cliente> PostCliente(ClienteDTO clienteDTO)
        {
            if (clienteDTO == null || !ValidateClienteDTO(clienteDTO))
            {
                // Adicione a lógica para lidar com a validação incorreta
                throw new ArgumentException("Parâmetros inválidos para criação de cliente.");
            }

            // Verifique se o CPF já está cadastrado
            Cliente existeCPF = await _clienteRepository.GetClienteByCpf(clienteDTO.CPF);
            if (existeCPF != null)
            {
                // Adicione a lógica para lidar com o CPF já cadastrado
                throw new InvalidOperationException("CPF já cadastrado para outro cliente.");
            }

            var cliente = MapClienteDtoToCliente(clienteDTO);
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


        private static bool ValidateClienteDTO(ClienteDTO clienteDTO)
        {
            return !string.IsNullOrEmpty(clienteDTO.Nome) &&
                   !string.IsNullOrEmpty(clienteDTO.Sobrenome) &&
                   !string.IsNullOrEmpty(clienteDTO.CPF) &&
                   !string.IsNullOrEmpty(clienteDTO.Email);
        }

        private static Cliente MapClienteDtoToCliente(ClienteDTO clienteDto)
        {
            return new Cliente
            {
                Nome = clienteDto.Nome,
                Sobrenome = clienteDto.Sobrenome,
                CPF = clienteDto.CPF,
                Email = clienteDto.Email
            };
        }
    }
}
