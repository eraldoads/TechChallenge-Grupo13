using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.EntitiesDTO;
using Domain.Port.DriverPort;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
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
        public async Task<ActionResult<IEnumerable<PedidoOutput>>> GetPedidos()
        {
            List<PedidoOutput> pedidos = await _pedidoService.GetPedidos();
            return pedidos;
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
            try
            {
                // Validação dos dados de entrada
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var novoPedido = await _pedidoService.PostPedido(pedidoInput);

                return CreatedAtAction("PostPedido", new { id = novoPedido.IdPedido }, novoPedido);
            }
            catch (PreconditionFailedException ex)
            {
                // Tratar a exceção aqui e retornar um BadRequest com a mensagem de erro personalizada
                return StatusCode(412, ex.Message);
            }
            catch (Exception)
            {
                // Tratar outras exceções, se necessário
                return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
            }
        }
    }
}
