using Domain.Entities;
using Domain.EntitiesDTO;
using Domain.Port.DrivenPort;
using Domain.Port.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;

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
            if (!ValidateClienteDTO(clienteDTO, out ICollection<ValidationResult> results))
            {
                var errors = results.Select(r => r.ErrorMessage).ToList();
                throw new ValidationException("Ocorreu um erro de validação", new Exception(string.Join("\n", errors)));
            }

            var existeCPF = await _clienteRepository.GetClienteByCpf(clienteDTO.CPF);

            if (existeCPF != null)
                throw new ValidationException("CPF já cadastrado para outro cliente.");

            var cliente = MapClienteDtoToCliente(clienteDTO);
            return await _clienteRepository.PostCliente(cliente);
        }

        public async Task PutCliente(int idCliente, Cliente clienteInput)
        {
            var cliente = await _clienteRepository.GetClienteById(idCliente) ?? throw new ResourceNotFoundException("Cliente não encontrado.");

            cliente.Email = clienteInput.Email;
            cliente.CPF = clienteInput.CPF;
            cliente.Nome = clienteInput.Nome;
            cliente.Sobrenome = clienteInput.Sobrenome;

            await UpdateCliente(cliente);
        }

        public async Task PatchCliente(int idCliente, JsonPatchDocument<Cliente> patchDoc)
        {
            var cliente = await _clienteRepository.GetClienteById(idCliente) ?? throw new ResourceNotFoundException("Cliente não encontrado.");
            patchDoc.ApplyTo(cliente);

            await UpdateCliente(cliente);
        }

        public async Task<int> DeleteCliente(int id)
        {
            return await _clienteRepository.DeleteCliente(id);
        }


        private async Task UpdateCliente(Cliente cliente)
        {
            if (!cliente.IsValid())
                throw new ValidationException("Dados inválidos.");

            await _clienteRepository.UpdateCliente(cliente);
        }

        private static bool ValidateClienteDTO(ClienteDTO clienteDTO, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(clienteDTO, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(clienteDTO, context, results, true);
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
