using Application.Interfaces;
using Domain.Entities.Input;
using Domain.Entities.Output;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [SwaggerResponse(204, "Requisição concluída sem dados de retorno.", null)]
    [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido à sintaxe malformada.", null)]
    [SwaggerResponse(401, "A requisição requer autenticação do usuário.", null)]
    [SwaggerResponse(403, "Privilégios insuficientes.", null)]
    [SwaggerResponse(404, "O recurso solicitado não existe.", null)]
    [SwaggerResponse(412, "Condição prévia dada em um ou mais dos campos avaliada como falsa.", null)]
    [SwaggerResponse(500, "O servidor encontrou uma condição inesperada.", null)]
    [Consumes("application/json")]

    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoController(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        // POST : /pagamento
        [HttpPost]
        [SwaggerOperation(
            Summary = "Endpoint para processar um pagamento",
            Description = @"Processa um pagamento com os dados recebidos no corpo da requisição </br>
                            <b>Parâmetros de entrada:</b>
                            <br/> • <b>idPedido</b>: o identificador do pedido relacionado ao pagamento ⇒ <font color='red'><b>Obrigatório</b></font>
                            <br/> • <b>valor</b>: o valor do pagamento ⇒ <font color='red'><b>Obrigatório</b></font>
                            <br/> • <b>metodoPagamento</b>: o método de pagamento escolhido pelo cliente ⇒ <font color='red'><b>Obrigatório</b></font>
                            <br/>",
            Tags = new[] { "Pagamentos" }
        )]
        [SwaggerResponse(201, "Pagamento processado com sucesso!", typeof(PagamentoOutput))]
        public async Task<IActionResult> ProcessarPagamento([Required][FromBody] PagamentoInput pagamentoInput)
        {
            try
            {
                // Validação dos dados de entrada
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var novoPagamento = await _pagamentoService.ProcessarPagamento(pagamentoInput);

                return CreatedAtAction("ProcessarPagamento", new { id = novoPagamento.IdPagamento }, novoPagamento);
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

        // POST : /pagamento/webhook
        [HttpPost("webhook")]
        [SwaggerOperation(
            Summary = "Endpoint para receber webhooks de pagamento",
            Description = "Recebe notificações assíncronas de pagamento do Mercado Pago.",
            Tags = new[] { "Pagamentos" }
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReceberWebhookPagamento([FromBody] PagamentoInput modeloWebhook)
        {
            try
            {
                // Lógica para validar e processar o webhook
                //await _pagamentoService.ProcessarWebhook(modeloWebhook);

                return Ok();
            }
            catch (Exception ex)
            {
                // Tratar exceções, se necessário
                return StatusCode(500, $"Erro ao processar webhook: {ex.Message}");
            }
        }
    }
}
