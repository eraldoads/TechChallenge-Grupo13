using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.EntitiesDTO;
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


        // PATCH : /pedido
        [HttpPatch("{idPedido}")]
        [SwaggerOperation(
            Summary = "Endpoint para atualizar o status de um pedido",
            Description = @"Atualiza o status de um pedido com o novo status fornecido </br>
                          <b>Parâmetros de entrada:</b>
                          <br/> • <b>idPedido</b>: o pedido a ser atualizado⇒ <font color='red'><b>Obrigatório</b></font>
                          <br/> • <b>statusPedido</b>: o status do pedido a ser atualizado, tem como definição os valores:⇒ <font color='red'><b>Obrigatório</b></font>
                              <br/>&nbsp;&emsp;&emsp;• <b>1</b> - Recebido
                              <br/>&nbsp;&emsp;&emsp;• <b>2</b> - Em Preparação
                              <br/>&nbsp;&emsp;&emsp;• <b>3</b> - Pronto
                              <br/>&nbsp;&emsp;&emsp;• <b>4</b> - Finalizado
                          <br/>",
            Tags = new[] { "Pedidos" }
        )]
        [SwaggerResponse(204, "Status do pedido atualizado com sucesso!")]
        public async Task<IActionResult> PatchStatusPedido([FromRoute] int idPedido, [FromBody] PedidoStatus novoStatus)
        {
            try
            {
                // Validação dos dados de entrada.
                // Retorna um código de status 400 se o modelo não for válido.
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Retorna um código de status 412 se o status do pedido for nulo
                if (novoStatus.StatusPedido == null)
                    return StatusCode(412, "O status do pedido é obrigatório");

                // Usa um método de extensão para obter a descrição do valor do enum StatusPedido
                string? statusDescricao = novoStatus.StatusPedido.GetDescription();

                // Trata a exceção gerada quando o enum é inválido
                // Retorna um código de status 412 se a descrição do enum for nula
                if (statusDescricao == null)
                    return StatusCode(412, $"Status do pedido {novoStatus.StatusPedido} é um valor inválido. Operação Cancelada. (Parameter '{nameof(PedidoStatus.StatusPedido)}')");

                //PreconditionFailedException($"Pedido com ID {idPedido} não existe. Operação Cancelada.", nameof(idPedido));

                // Chama o método do serviço de pedido para atualizar o status do pedido no banco de dados.
                var success = await _pedidoService.UpdateStatusPedido(idPedido, statusDescricao);

                return success
                    ? NoContent() // Retorna um código de status 204 se a atualização foi bem-sucedida.
                    : StatusCode(500, "Ocorreu um erro ao tentar atualizar o status do pedido."); // Retorna um código de status 500 se a atualização falhar.

            }
            catch (PreconditionFailedException ex)
            {
                return StatusCode(412, ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { idPedido, error = "Status do pedido não encontrado" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
            }
        }

    }
}
