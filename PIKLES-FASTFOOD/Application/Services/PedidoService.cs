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

        public PedidoService(IPedidoRepository pedidoRepository, IProdutoService produtoService)
        {
            _pedidoRepository = pedidoRepository;
            _produtoService = produtoService;
        }

        public async Task<List<PedidoOutput>> GetPedidos()
        {
            return await _pedidoRepository.GetPedidos();
        }

        public async Task<PedidoDTO> PostPedido(PedidoInput pedidoInput)
        {            
            var novoPedido = new Pedido
            {
                IdCliente = pedidoInput.IdCliente,
                DataPedido = pedidoInput.DataPedido,
                StatusPedido = pedidoInput.StatusPedido,
                ValorTotal = 0
            };            
            
            foreach (var comboInput in pedidoInput.Combo)
            {
                var novoCombo = new Combo();                

                foreach (var produtoInput in comboInput.Produto)
                {                
                    var produto = await _produtoService.GetProdutoById(produtoInput.IdProduto);                    

                    var novoProdutoCombo = new ComboProduto
                    {
                        IdProduto = produtoInput.IdProduto,
                        Quantidade = produtoInput.Quantidade
                    };
                    
                    novoCombo.Produtos.Add(novoProdutoCombo);                    
                    novoPedido.ValorTotal += produto.ValorProduto * produtoInput.Quantidade;
                }
             
                novoPedido.Combos.Add(novoCombo);
            }

            await _pedidoRepository.PostPedido(novoPedido);            
            
            return new PedidoDTO
            {
                IdPedido = novoPedido.IdPedido,
                DataPedido = novoPedido.DataPedido,
                ValorTotal = novoPedido.ValorTotal
            };
        }
    }    
}
