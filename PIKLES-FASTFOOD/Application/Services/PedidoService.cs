using Domain.Entities;
using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.EntitiesDTO;
using Domain.Port.DrivenPort;
using Domain.Port.DriverPort;
using Domain.Port.Services;

namespace Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoService _produtoService;

        /// <summary>
        /// Construtor para a classe PedidoService.
        /// </summary>
        /// <param name="pedidoRepository">O repositório de pedidos a ser usado pela classe PedidoService.</param>
        /// <param name="produtoService">O serviço de produtos a ser usado pela classe PedidoService.</param>
        public PedidoService(IPedidoRepository pedidoRepository, IProdutoService produtoService)
        {
            _pedidoRepository = pedidoRepository;
            _produtoService = produtoService;
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
            var novoPedido = new Pedido
            {
                IdCliente = pedidoInput.IdCliente,
                DataPedido = pedidoInput.DataPedido,
                StatusPedido = pedidoInput.StatusPedido,
            };

            foreach (var comboInput in pedidoInput.Combo ?? new List<ComboInput>())
            {
                var novoCombo = new Combo();
                if (comboInput.Produto is not null)
                {
                    foreach (var produtoInput in comboInput.Produto)
                    {
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
