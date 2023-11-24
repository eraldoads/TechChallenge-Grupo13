﻿using Domain.Entities;
using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.EntitiesDTO;
using Domain.Port.DrivenPort;
using Domain.Port.DriverPort;
using Domain.Port.Services;
using Domain.ValueObjects;

namespace Application.Services
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
    }
}
