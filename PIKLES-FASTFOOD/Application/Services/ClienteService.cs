using Domain.Entities;
using Domain.EntitiesDTO;
using Domain.Interfaces;
//using Application.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;

namespace Application.Interfaces
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        /// <summary>
        /// Construtor para a classe ClienteService.
        /// </summary>
        /// <param name="clienteRepository">O repositório de clientes a ser usado pela classe ClienteService.</param>
        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        /// <summary>
        /// Obtém um cliente pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do cliente.</param>
        /// <returns>Retorna o cliente se encontrado; caso contrário, retorna null.</returns>
        public async Task<Cliente?> GetClienteById(int id)
        {
            var cliente = await _clienteRepository.GetClienteById(id);

            return cliente;
        }

        /// <summary>
        /// Obtém um cliente pelo seu CPF.
        /// </summary>
        /// <param name="cpf">O CPF do cliente.</param>
        /// <returns>Retorna o cliente se encontrado.</returns>
        public async Task<Cliente?> GetClienteByCpf(string cpf)
        {
            return await _clienteRepository.GetClienteByCpf(cpf);
        }

        /// <summary>
        /// Obtém todos os clientes.
        /// </summary>
        /// <returns>Retorna uma lista de clientes.</returns>
        public async Task<List<Cliente>> GetClientes()
        {
            return await _clienteRepository.GetClientes();
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="clienteDTO">O DTO do cliente a ser criado.</param>
        /// <returns>Retorna o cliente criado.</returns>
        public async Task<Cliente> PostCliente(ClienteDTO clienteDTO)
        {
            if (!ValidateClienteDTO(clienteDTO, out ICollection<ValidationResult> results))
            {
                var errors = results.Select(r => r.ErrorMessage).ToList();
                throw new ValidationException("Ocorreu um erro de validação", new Exception(string.Join("\n", errors)));
            }

            if (clienteDTO.CPF is null)
                throw new ArgumentNullException(nameof(clienteDTO.CPF), "O CPF não pode ser nulo");

            var existeCPF = await _clienteRepository.GetClienteByCpf(clienteDTO.CPF);

            if (existeCPF is not null)
                throw new ValidationException("CPF já cadastrado para outro cliente.");

            var cliente = MapClienteDtoToCliente(clienteDTO);
            return await _clienteRepository.PostCliente(cliente);
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        /// <param name="idCliente">O ID do cliente a ser atualizado.</param>
        /// <param name="clienteInput">O cliente com as informações atualizadas.</param>
        public async Task PutCliente(int idCliente, Cliente clienteInput)
        {
            var cliente = await _clienteRepository.GetClienteById(idCliente) ?? throw new ResourceNotFoundException("Cliente não encontrado.");

            cliente.Email = clienteInput.Email;
            cliente.CPF = clienteInput.CPF;
            cliente.Nome = clienteInput.Nome;
            cliente.Sobrenome = clienteInput.Sobrenome;

            await UpdateCliente(cliente);
        }

        /// <summary>
        /// Atualiza parcialmente um cliente existente.
        /// </summary>
        /// <param name="idCliente">O ID do cliente a ser atualizado.</param>
        /// <param name="patchDoc">O JsonPatchDocument com as operações de atualização.</param>
        public async Task PatchCliente(int idCliente, JsonPatchDocument<Cliente> patchDoc)
        {
            var cliente = await _clienteRepository.GetClienteById(idCliente) ?? throw new ResourceNotFoundException("Cliente não encontrado.");
            patchDoc.ApplyTo(cliente);

            await UpdateCliente(cliente);
        }

        /// <summary>
        /// Exclui um cliente existente.
        /// </summary>
        /// <param name="id">O ID do cliente a ser excluído.</param>
        /// <returns>Retorna o cliente excluído.</returns>
        public async Task<Cliente> DeleteCliente(int id)
        {
            try
            {
                var cliente = await GetClienteById(id) ?? throw new KeyNotFoundException("Cliente não encontrado.");
                await _clienteRepository.DeleteCliente(id);

                return cliente;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }


        // Métodos Privados.

        #region [Métodos Privados]
        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        /// <param name="cliente">O cliente com as informações atualizadas.</param>
        private async Task UpdateCliente(Cliente cliente)
        {
            if (!cliente.IsValid())
                throw new ValidationException("Dados inválidos.");

            await _clienteRepository.UpdateCliente(cliente);
        }

        /// <summary>
        /// Valida um ClienteDTO.
        /// </summary>
        /// <param name="clienteDTO">O ClienteDTO a ser validado.</param>
        /// <param name="results">A coleção de resultados de validação.</param>
        /// <returns>Retorna verdadeiro se o ClienteDTO for válido; caso contrário, retorna falso.</returns>
        private static bool ValidateClienteDTO(ClienteDTO clienteDTO, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(clienteDTO, serviceProvider: null, items: null);
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(clienteDTO, context, results, true);
        }

        /// <summary>
        /// Mapeia um ClienteDTO para um Cliente.
        /// </summary>
        /// <param name="clienteDto">O ClienteDTO a ser mapeado.</param>
        /// <returns>Retorna um Cliente que corresponde ao ClienteDTO.</returns>
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
        #endregion

    }
}
