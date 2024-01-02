using Domain.Entities;
using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.EntitiesDTO;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.Interfaces
{
    public class PedidoService : DateTimeAdjuster, IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        /// <summary>
        /// Construtor para a classe PedidoService.
        /// </summary>
        /// <param name="pedidoRepository">O repositório de pedidos a ser usado pela classe PedidoService.</param>
        /// <param name="produtoService">O serviço de produtos a ser usado pela classe PedidoService.</param>
        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        /// <summary>
        /// Obtém todos os pedidos.
        /// </summary>
        /// <returns>Retorna uma lista de pedidos.</returns>
        public async Task<List<PedidoOutput>> GetPedidos()
        {
            return await _pedidoRepository.GetPedidos();
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <param name="pedidoInput">O DTO do pedido a ser criado.</param>
        /// <returns>Retorna o pedido criado.</returns>
        public async Task<PedidoDTO> PostPedido(PedidoInput pedidoInput)
        {
            DateTime dataPedidoAjustada = AjustaDataHoraLocal(pedidoInput);

            var novoPedido = new Pedido
            {
                IdCliente = pedidoInput.IdCliente,
                DataPedido = dataPedidoAjustada, // usa a data ajustada
                StatusPedido = pedidoInput.StatusPedido,
            };

            foreach (var comboInput in pedidoInput.Combo ?? new List<ComboInput>())
            {
                var novoCombo = new Combo();
                if (comboInput.Produto is not null)
                {
                    foreach (var produtoInput in comboInput.Produto)
                    {
                        // Verifica se o produto existe
                        if (!await _pedidoRepository.ProdutoExists(produtoInput.IdProduto))
                        {
                            throw new PreconditionFailedException($"Produto com ID {produtoInput.IdProduto} não existe. Operação cancelada", nameof(produtoInput.IdProduto));
                        }

                        var novoProdutoCombo = new ComboProduto
                        {
                            IdProduto = produtoInput.IdProduto,
                            Quantidade = produtoInput.Quantidade
                        };

                        novoCombo.Produtos.Add(novoProdutoCombo);
                    }
                }
                novoPedido.Combos.Add(novoCombo);
            }

            // Verifica se o cliente existe
            if (!await _pedidoRepository.ClienteExists(novoPedido.IdCliente))
            {
                throw new PreconditionFailedException($"Cliente com ID {novoPedido.IdCliente} não existe. Operação cancelada", nameof(novoPedido.IdCliente));
            }

            var novoPedidoCriado = await _pedidoRepository.PostPedido(novoPedido);

            return new PedidoDTO
            {
                IdPedido = novoPedidoCriado.IdPedido,
                DataPedido = novoPedidoCriado.DataPedido,
                ValorTotal = novoPedidoCriado.ValorTotal
            };
        }

        /// <summary>
        /// Atualiza o status de um pedido no banco de dados.
        /// </summary>
        /// <param name="idPedido">O id do pedido a ser atualizado.</param>
        /// <param name="novoStatus">O novo status do pedido.</param>
        /// <returns>Um valor booleano que indica se a atualização foi bem-sucedida ou não.</returns>
        /// <exception cref="PreconditionFailedException">Se o pedido com o id especificado não existir.</exception>
        /// <exception cref="DbUpdateException">Se ocorrer um erro ao atualizar o banco de dados.</exception>
        public async Task<bool> UpdateStatusPedido(int idPedido, string novoStatus)
        {
            // obtém o pedido pelo seu id ou lança uma exceção se não existir/
            var pedido = await _pedidoRepository.GetPedidoById(idPedido) ?? throw new PreconditionFailedException($"Pedido com ID {idPedido} não existe. Operação Cancelada.", nameof(idPedido));

            // altera o status do pedido para o novo status/
            pedido.StatusPedido = novoStatus;
            // atualiza o pedido no banco de dados/
            await _pedidoRepository.UpdatePedido(pedido);

            // retorna verdadeiro se a atualização foi bem-sucedida/
            return true;
        }

    }
}
