using Data.Context;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

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

        // GET : /api/pedido
        [HttpGet()]
        [SwaggerOperation(
            Summary = "Endpoint para retornar com todos os pedidos realizados",
            Description = @"Busca todos os pedidos realizados </br>",
            Tags = new[] { "Pedidos" }
        )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(List<PedidoDTO>))]
        [SwaggerResponse(204, "Requisição concluída sem dados de retorno.", null)]
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
                        select new PedidoDTO
                        {
                            Cliente = cliente.Nome + " " + cliente.Sobrenome,
                            DataPedido = pedido.DataPedido,
                            Quantidade = pedido_produto.Quantidade,
                            NomeCategoria = categoria.NomeCategoria,
                            CodigoProduto = produto.CodigoProduto,
                            NomeProduto = produto.NomeProduto,
                            ValorProduto = produto.ValorProduto
                        };
            query = query.OrderBy(p => p.DataPedido);

            // Verifica se a consulta retornou algum resultado
            var result = await query.ToListAsync();
            
            if (result == null || result.Count == 0)
                // Retorna o status code 204 (No Content)
                return NoContent();
            else
                // Retorna o status code 200 (OK) com o resultado no corpo da resposta
                return Ok(result);
        }

        // POST : /api/pedido
        [HttpPost]
        [SwaggerOperation(
            Summary = "Endpoint para criar um novo pedido",
            Description = @"Cria um novo pedido com os dados recebidos no corpo da requisição </br>
                    <b>Parâmetros de entrada:</b>
                    <br/> • <b>idCliente</b>: o identificador do cliente que realizou o pedido ⇒ <font color='red'><b>Obrigatório</b></font>
                    <br/> • <b>dataPedido</b>: a data em que o pedido foi realizado ⇒ <font color='red'><b>Obrigatório</b></font>
                    <br/> • <b>produtos</b>: uma lista de produtos incluídos no pedido, cada um contendo os seguintes parâmetros:
                        <br/>&nbsp;&emsp; • <b>IdProduto</b>: o identificador do produto incluído no pedido ⇒ <font color='red'><b>Obrigatório</b></font>
                        <br/>&nbsp;&emsp; • <b>Quantidade</b>: a quantidade do respectivo produto incluído no pedido ⇒ <font color='red'><b>Obrigatório</b></font>
                    ",
            Tags = new[] { "Pedidos" }
        )]
        [SwaggerResponse(201, "Pedido criado com sucesso!", typeof(Pedido))]
        [SwaggerResponse(400, "Dados inválidos ou incompletos!", null)]
        [SwaggerResponse(500, "Erro interno no servidor!", null)]
        public async Task<IActionResult> PostPedido([Required][FromBody] PedidoInput pedidoInput)
        {
            // Validação dos dados de entrada
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pedidoInput.Produtos == null)
                return BadRequest("Lista de produtos está vazia!");

            try
            {
                if (_context is null || _context.Produto is null || _context.Pedido_Produto is null || _context.Pedido is null)
                    return StatusCode(500, "Ocorreu um erro interno no servidor. Entre em contato com o suporte técnico.");

                var pedido = new Pedido
                {
                    IdCliente = pedidoInput.IdCliente,
                    DataPedido = pedidoInput.DataPedido,
                    ValorTotal = 0
                };

                _context.Pedido.Add(pedido);
                await _context.SaveChangesAsync();

                var pedidoProdutos = new List<Pedido_Produto>();

                foreach (var produto in pedidoInput.Produtos)
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
