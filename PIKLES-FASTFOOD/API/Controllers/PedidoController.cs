using Data.Context;
using Domain.Entities;
using Domain.Entities.Input;
using Domain.Entities.Output;
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
            Summary = "Endpoint para retornar todos os pedidos realizados",
            Description = @"Busca todos os pedidos realizados </br>",
            Tags = new[] { "Pedidos" }
        )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(List<PedidoOutput>))]
        [SwaggerResponse(206, "Conteúdo Parcial!", typeof(List<PedidoOutput>))]
        public ActionResult<IEnumerable<PedidoOutput>> GetPedido()
        {
            if (_context is null || _context.Pedido is null || _context.Cliente is null || _context.Produto is null || _context.Categoria is null)
                return StatusCode(500, "Ocorreu um erro interno no servidor. Entre em contato com o suporte técnico.");

            var pedidos = new List<PedidoOutput>();
            var pedidosAgrupados = new Dictionary<int, PedidoOutput>();

            var query = @"  SELECT  PEDI.IdPedido                           AS idPedido,
                                    PEDI.DataPedido                         AS dataPedido,
                                    PEDI.StatusPedido                       AS statusPedido,
                                    CONCAT(CLIE.Nome, ' ', CLIE.Sobrenome)  AS nomeCompletoCliente,
                                    PEDI.ValorTotal                         AS valorTotalPedido,
                                    COMB.IdCombo                            AS idCombo,
                                    PROD.IdProduto                          AS idProduto,
                                    PROD.NomeProduto                        AS nomeProduto,
                                    COMBP.Quantidade                        AS quantidadeProduto,
                                    PROD.ValorProduto                       AS valorProduto
                               FROM Pedido PEDI
                         INNER JOIN Cliente CLIE
                                 ON PEDI.IdCliente = CLIE.IdCliente
                         INNER JOIN Combo COMB
                                 ON COMB.PedidoId = PEDI.IdPedido
                         INNER JOIN ComboProduto COMBP
                                 ON COMBP.ComboId = COMB.IdCombo
                         INNER JOIN Produto PROD
                                 ON COMBP.IdProduto = PROD.IdProduto
                           ORDER BY dataPedido, idCombo, nomeCompletoCliente;
                        ";

            // Execute a query SQL para obter os pedidos e suas informações relacionadas
            var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandText = query;
            _context.Database.OpenConnection();

            using (var result = command.ExecuteReader())
            {
                while (result.Read())
                {
                    var idPedido = Convert.ToInt32(result["idPedido"]);

                    if (!pedidosAgrupados.ContainsKey(idPedido))
                    {
                        var pedidoOutput = new PedidoOutput
                        {
                            IdPedido = idPedido,
                            DataPedido = Convert.ToDateTime(result["dataPedido"]),
                            StatusPedido = result["statusPedido"].ToString(),
                            NomeCliente = result["nomeCompletoCliente"].ToString(),
                            ValorTotalPedido = Convert.ToSingle(result["valorTotalPedido"]),
                            Combo = new List<ComboOutput>()
                        };

                        pedidosAgrupados[idPedido] = pedidoOutput;
                    }

                    if (pedidosAgrupados.TryGetValue(idPedido, out PedidoOutput? pedidoAgrupado))
                    {
                        if (pedidoAgrupado != null && pedidoAgrupado.Combo != null)
                        {
                            var idCombo = Convert.ToInt32(result["idCombo"]);
                            var produtoOutput = new ProdutoOutput
                            {
                                IdProduto = Convert.ToInt32(result["idProduto"]),
                                NomeProduto = result["nomeProduto"].ToString(),
                                QuantidadeProduto = Convert.ToInt32(result["quantidadeProduto"]),
                                ValorProduto = Convert.ToSingle(result["valorProduto"])
                            };

                            if (pedidoAgrupado.Combo.Any(c => c.IdCombo == idCombo))
                            {
                                var comboOutput = pedidoAgrupado.Combo.FirstOrDefault(c => c.IdCombo == idCombo);
                                if (comboOutput is null || comboOutput.Produto is null)
                                    return StatusCode(500, "Ocorreu um erro interno no servidor. Entre em contato com o suporte técnico.");

                                comboOutput?.Produto.Add(produtoOutput);
                            }
                            else
                            {
                                var comboOutput = new ComboOutput
                                {
                                    IdCombo = idCombo,
                                    Produto = new List<ProdutoOutput> { produtoOutput }
                                };
                                pedidoAgrupado.Combo.Add(comboOutput);
                            }
                        }
                    }
                }
            }

            _context.Database.CloseConnection();

            pedidos.AddRange(pedidosAgrupados.Values);

            // Verifique se há pedidos e retorne uma resposta adequada
            if (pedidos.Count == 0)
                return StatusCode(204);

            return Ok(pedidos);
        }


        // POST : /pedido
        [HttpPost]
        [SwaggerOperation(
            Summary = "Endpoint para criar um novo pedido",
            Description = @"Cria um novo pedido com os dados recebidos no corpo da requisição </br>
                <b>Parâmetros de entrada:</b>
                <br/> • <b>idCliente</b>: o identificador do cliente que realizou o pedido ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>dataPedido</b>: a data em que o pedido foi realizado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>statusPedido</b>: o status atual do pedido ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>combo</b>: uma lista de combos incluídos no pedido, cada um contendo uma lista de produtos:
                    <br/>&nbsp;&emsp;&emsp;  • <b>produto</b>: a lista de produtos incluídos no combo, cada um contendo os seguintes parâmetros:
                        <br/>&nbsp;&emsp;&emsp;&nbsp;&emsp;&emsp;   • <b>idProduto</b>: o identificador do produto incluído no combo ⇒ <font color='red'><b>Obrigatório</b></font>
                        <br/>&nbsp;&emsp;&emsp;&nbsp;&emsp;&emsp;   • <b>quantidade</b>: a quantidade do respectivo produto incluído no combo ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Pedidos" }
        )]

        [SwaggerResponse(201, "Pedido criado com sucesso!", typeof(PedidoDTO))]
        public async Task<IActionResult> PostPedido([Required][FromBody] PedidoInput pedidoInput)
        {
            // Validação dos dados de entrada
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Criando um novo objeto Pedido com base nos dados fornecidos
            var novoPedido = new Pedido
            {
                IdCliente = pedidoInput.IdCliente,
                DataPedido = pedidoInput.DataPedido,
                StatusPedido = pedidoInput.StatusPedido,
                ValorTotal = 0
            };

            if (_context is null || _context.Produto is null || _context.Pedido is null || pedidoInput.Combo is null)
                return StatusCode(500, "Ocorreu um erro interno no servidor. Entre em contato com o suporte técnico.");

            // Adicionando combos e produtos ao pedido
            foreach (var comboInput in pedidoInput.Combo)
            {
                var novoCombo = new Combo();

                if (comboInput.Produto is null)
                    return StatusCode(500, "Ocorreu um erro interno no servidor. Entre em contato com o suporte técnico.");

                // Adicionar produtos ao combo e calcular o valor total do pedido
                foreach (var produtoInput in comboInput.Produto)
                {
                    var produto = await _context.Produto.FindAsync(produtoInput.IdProduto);
                    if (produto == null)
                        return StatusCode(404, $"Produto com Id {produtoInput.IdProduto} não encontrado.");

                    var novoProdutoCombo = new ComboProduto
                    {
                        IdProduto = produtoInput.IdProduto,
                        Quantidade = produtoInput.Quantidade
                    };

                    // Adicionar novoProdutoCombo à lista de produtos no novoCombo
                    novoCombo.Produtos.Add(novoProdutoCombo);

                    // Atualizar o valor total do pedido com o valor do produto atual
                    novoPedido.ValorTotal += produto.ValorProduto * produtoInput.Quantidade;
                }

                // Adicionar novoCombo ao novoPedido
                novoPedido.Combos.Add(novoCombo);
            }

            // Adicionar o novo pedido ao contexto
            _context.Pedido.Add(novoPedido);

            try
            {
                // Salvar as mudanças no banco de dados
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Se houver um erro ao salvar no banco de dados, retorne um erro 500
                return StatusCode(500, "Ocorreu um erro ao salvar o pedido no banco de dados.");
            }

            // Retornar o novo pedido criado
            return StatusCode(201, new PedidoDTO
            {
                IdPedido = novoPedido.IdPedido,
                DataPedido = novoPedido.DataPedido,
                ValorTotal = novoPedido.ValorTotal
            });
        }


    }
}
