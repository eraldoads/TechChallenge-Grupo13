using Data.Context;
using Domain.Entities;
using Domain.EntitiesDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    [Produces("application/json", new string[] { })]
    [SwaggerResponse(204, "Requisição concluída sem dados de retorno.", null)]
    [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido à sintaxe malformada.", null)]
    [SwaggerResponse(401, "A requisição requer autenticação do usuário.", null)]
    [SwaggerResponse(403, "Privilégios insuficientes.", null)]
    [SwaggerResponse(404, "O recurso solicitado não existe.", null)]
    [SwaggerResponse(412, "Condição prévia dada em um ou mais dos campos avaliada como falsa.", null)]
    [SwaggerResponse(500, "O servidor encontrou uma condição inesperada.", null)]
    [Consumes("application/json", new string[] { })]

    public class PedidoController : ControllerBase
    {
        private readonly MySQLContext _context;

        public PedidoController(MySQLContext context)
        {
            _context = context;
        }

        // GET : /pedido
        [HttpGet()]
        [SwaggerOperation(
            Summary = "Endpoint para retornar com todos os pedidos realizados",
            Description = @"Busca todos os pedidos realizados </br>",
            Tags = new[] { "Pedidos" }
        )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(List<PedidoDTO>))]
        [SwaggerResponse(206, "Conteúdo Parcial!", typeof(List<PedidoDTO>))]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedido()
        {
            if (_context.Pedido is null || _context.Pedido_Produto is null || _context.Cliente is null || _context.Produto is null || _context.Categoria is null)
                return StatusCode(500, "Ocorreu um erro interno no servidor. Entre em contato com o suporte técnico.");

            var query = from pedido in _context.Pedido
                        join cliente in _context.Cliente on pedido.IdCliente equals cliente.Id
                        join pedido_produto in _context.Pedido_Produto on pedido.Id equals pedido_produto.IdPedido
                        join produto in _context.Produto on pedido_produto.IdProduto equals produto.Id
                        join categoria in _context.Categoria on (int)produto.IdCategoria equals categoria.Id
                        select new
                        {
                            Pedido = pedido,
                            Cliente = cliente,
                            PedidoProduto = pedido_produto,
                            Produto = produto,
                            Categoria = categoria
                        };
            query = query.Distinct();

            var groupedQuery = query.GroupBy(
                        p => new
                        {
                            p.Pedido.Id,
                            p.Cliente.Nome,
                            p.Cliente.Sobrenome,
                            p.Pedido.DataPedido,
                            p.Pedido.StatusPedido,
                            p.Pedido.ValorTotal
                        },
                        (key, items) => new PedidoDTO
                        {
                            IdPedido = key.Id,
                            DataPedido = key.DataPedido,
                            StatusPedido = key.StatusPedido,
                            NomeCliente = key.Nome + " " + key.Sobrenome,
                            ValorTotalPedido = key.ValorTotal,
                            // Lista para armazenar múltiplos combos.
                            Combo = items.GroupBy(
                                item => new { item.PedidoProduto.IdPedido },
                                (itemKey, itemGroup) => new ComboDTO
                                {
                                    Produto = itemGroup.Select(prod => new ProdutoDTO
                                    {
                                        IdProduto = prod.Produto.Id,
                                        NomeProduto = prod.Produto.NomeProduto,
                                        QuantidadeProduto = prod.PedidoProduto.Quantidade,
                                        ValorProduto = prod.Produto.ValorProduto
                                    }).ToList()
                                }
                            ).ToList()
                        });

            var result = await groupedQuery.ToListAsync();

            if (result == null || result.Count == 0)
                return NoContent();
            else
                return Ok(result);
        }


        // POST : /pedido
        [HttpPost]
        [SwaggerOperation(
            Summary = "Endpoint para criar um novo pedido",
            Description = @"Cria um novo pedido com os dados recebidos no corpo da requisição </br>
                <b>Parâmetros de entrada:</b>
                <br/> • <b>idCliente</b>: o identificador do cliente que realizou o pedido ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>dataPedido</b>: a data em que o pedido foi realizado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>statusPedido</b>: o status do pedido ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>combo</b>: uma lista de combos incluídos no pedido, cada um contendo uma lista de produtos:
                    <br/>&nbsp;&emsp; • <b>produto</b>: a lista de produtos incluídos no combo, cada um contendo os seguintes parâmetros:
                        <br/>&nbsp;&emsp;&emsp; • <b>IdProduto</b>: o identificador do produto incluído no combo ⇒ <font color='red'><b>Obrigatório</b></font>
                        <br/>&nbsp;&emsp;&emsp; • <b>Quantidade</b>: a quantidade do respectivo produto incluído no combo ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Pedidos" }
        )]
        [SwaggerResponse(201, "Pedido criado com sucesso!", typeof(Pedido))]
        public async Task<IActionResult> PostPedido([Required][FromBody] PedidoInput pedidoInput)
        {
            // Validação dos dados de entrada
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pedidoInput.Combo == null)
                return BadRequest("Lista de combo está vazia!");

            try
            {
                if (_context is null || _context.Produto is null || _context.Pedido_Produto is null || _context.Pedido is null)
                    return StatusCode(500, "Ocorreu um erro interno no servidor. Entre em contato com o suporte técnico.");

                var pedido = new Pedido
                {
                    IdCliente = pedidoInput.IdCliente,
                    DataPedido = pedidoInput.DataPedido,
                    StatusPedido = pedidoInput.StatusPedido,
                    ValorTotal = 0
                };

                _context.Pedido.Add(pedido);
                await _context.SaveChangesAsync();

                var pedidoProdutos = new List<Pedido_Produto>();

                foreach (var combo in pedidoInput.Combo)
                {
                    if (combo.Produto == null)
                    {
                        return BadRequest("Lista de produtos no combo está vazia!");
                    }

                    foreach (var produto in combo.Produto)
                    {
                        var produtoDb = await _context.Produto.AsNoTracking().FirstOrDefaultAsync(p => p.Id == produto.IdProduto);

                        if (produtoDb is null || produto.Quantidade <= 0)
                            return BadRequest("Produto e/ou quantidade inválida!");

                        var pedidoProduto = new Pedido_Produto
                        {
                            IdPedido = pedido.Id,
                            IdProduto = produto.IdProduto,
                            Quantidade = produto.Quantidade
                        };

                        pedidoProdutos.Add(pedidoProduto);
                        pedido.ValorTotal += produtoDb.ValorProduto * produto.Quantidade;
                    }
                }

                _context.Pedido_Produto.AddRange(pedidoProdutos);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro: {ex.Message}");
            }
        }

    }
}
