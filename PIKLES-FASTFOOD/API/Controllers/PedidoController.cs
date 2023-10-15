using Data.Context;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;


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
            if (_context.Pedido == null || _context.Pedido_Produto == null)
                return NoContent();

            var query = from pedido in _context.Pedido
                        join cliente in _context.Cliente on pedido.IdCliente equals cliente.Id
                        join pedido_produto in _context.Pedido_Produto on pedido.Id equals pedido_produto.IdPedido
                        join produto in _context.Produto on pedido_produto.IdProduto equals produto.Id
                        join categoria in _context.Categoria on (int)produto.IdCategoria equals categoria.Id
                        select new PedidoDTO
                        {
                            Cliente = cliente.Nome + " " + cliente.Sobrenome,
                            Quantidade = pedido_produto.Quantidade,
                            NomeCategoria = categoria.NomeCategoria,
                            CodigoProduto = produto.CodigoProduto,
                            NomeProduto = produto.NomeProduto,
                            ValorProduto = produto.ValorProduto
                        };

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
        Description = @"Cria um novo pedido com os dados recebidos no corpo da requisição </br>",
        Tags = new[] { "Pedidos" }
        )]
        [SwaggerResponse(201, "Pedido criado com sucesso!", typeof(Pedido))]
        [SwaggerResponse(400, "Dados inválidos ou incompletos!", null)]
        [SwaggerResponse(500, "Erro interno no servidor!", null)]
        public async Task<IActionResult> PostPedido([FromBody] PedidoInput input)
        {
            // Valida os dados de entrada
            if (input == null || input.IdCliente <= 0 || input.DataPedido == null || input.Produtos == null || input.Produtos.Count == 0)
            {
                return BadRequest("Dados inválidos ou incompletos!");
            }

            try
            {
                // Cria uma instância da entidade Pedido com os dados de entrada
                var pedido = new Pedido
                {
                    IdCliente = input.IdCliente,
                    DataPedido = input.DataPedido,
                    ValorTotal = 0
                };

                // Adiciona o pedido ao contexto do banco de dados
                _context.Pedido.Add(pedido);

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();

                // Cria uma lista para armazenar os itens Pedido_Produto a serem adicionados
                List<Pedido_Produto> pedidoProdutos = new List<Pedido_Produto>();

                // Percorre a lista de produtos do pedido
                foreach (var produto in input.Produtos)
                {
                    // Busca o produto pelo id no banco de dados
                    //var produtoDb = await _context.Produto.FindAsync(produto.IdProduto);
                    var produtoDb = await _context.Produto.AsNoTracking().FirstOrDefaultAsync(p => p.Id == produto.IdProduto);


                    // Verifica se o produto existe e se a quantidade é válida
                    if (produtoDb == null || produto.Quantidade <= 0)
                    {
                        return BadRequest("Produto inválido ou quantidade inválida!");
                    }

                    // Cria uma instância da entidade Pedido_Produto com os dados do produto e do pedido
                    var pedidoProduto = new Pedido_Produto
                    {
                        IdPedido = pedido.Id,
                        IdProduto = produto.IdProduto,
                        Quantidade = produto.Quantidade
                    };

                    // Adiciona o item à lista
                    pedidoProdutos.Add(pedidoProduto);

                    // Atualiza o valor total do pedido
                    pedido.ValorTotal += produtoDb.ValorProduto * produto.Quantidade;
                }

                // Adiciona os itens Pedido_Produto ao contexto do banco de dados de forma assíncrona
                await _context.Pedido_Produto.AddRangeAsync(pedidoProdutos);

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();

                // Retorna o código de status 201 (Created) com o pedido criado no corpo da resposta
                return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
            }
            catch (Exception ex)
            {
                // Retorna o código de status 500 (Internal Server Error) com a mensagem de erro no corpo da resposta
                return StatusCode(500, ex.Message);
            }
        }


    }
}
