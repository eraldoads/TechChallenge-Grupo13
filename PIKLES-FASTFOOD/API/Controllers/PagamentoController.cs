using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Output;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        // GET : /pagamento/{id}/status
        [HttpGet("{idPedido}/status")]
        [SwaggerOperation(
            Summary = "Endpoint para obter o status do pagamento de um pedido",
            Description = @"Endpoint para obter o status do pagamento de um determinado pedido </br>
        <b>Parâmetros de entrada:</b>
        <br/> • <b>idPedido</b>: o identificador do pedido ⇒ <font color='red'><b>Obrigatório</b></font>
        ",
            Tags = new[] { "Pagamento" }
        )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(PagamentoStatusOutput))]
        public async Task<ActionResult<PagamentoStatusOutput?>> GetStatusPagamentoPedido(int idPedido)
        {
            try
            {
                // Chamar o serviço para obter o status do pagamento
                PagamentoStatusOutput? statusPagamento = await _pagamentoService.GetStatusPagamento(idPedido);

                if (statusPagamento is null)
                    return NoContent();

                return statusPagamento;
            }
            catch (Exception)
            {
                // Tratar outras exceções, se necessário
                return StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
            }
        }

        // POST : /pagamento
        [HttpPost]
        [SwaggerOperation(
            Summary = "Endpoint para processar o pagamento de um pedido",
            Description = @"Endpoint para processar o pagamento de um determinado pedido </br>
                            <b>Parâmetros de entrada:</b>
                            <br/> • <b>StatusPagamento</b>: o identificador do Status do pagamento. Por padrão inicia como Pendente. ⇒ <font color='red'><b>Obrigatório</b></font>
                            <br/> • <b>ValorPagamento</b>: o valor do pagamento. ⇒ <font color='red'><b>Obrigatório</b></font>
                            <br/> • <b>MetodoPagamento</b>: o método de pagamento escolhido pelo cliente. Exemplo: QRCode, cartão de crédito, boleto, etc. Se não for informado, por padrão é gravado QRCode ⇒ <font color='green'><b>Opcional</b></font>
                            <br/> • <b>DataPagamento</b>: Data do do pagamento. ⇒ <font color='red'><b>Obrigatório</b></font>
                            <br/> • <b>IdPedido</b>: o identificador do pedido relacionado ao pagamento. ⇒ <font color='red'><b>Obrigatório</b></font>
                            <br/>",
            Tags = new[] { "Pagamento" }
        )]
        [SwaggerResponse(201, "Pagamento processado com sucesso!", typeof(PagamentoOutput))]
        public async Task<ActionResult<Pagamento>> ProcessarPagamento([FromBody] Pagamento pagamentoInput)
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
            Summary = "Endpoint para receber notificações de pagamento",
            Description = @"Recebe notificações assíncronas de pagamento do Mercado Pago </br>                        
            <b>Parâmetros de entrada:</b>
            <br/> • <b>id</b>: o identificador da ordem de pagamento no Mercado Pago ⇒ <font color='red'><b>Obrigatório</b></font>
            <br/> • <b>topic</b>: este endpoint somente tratará requisições com o parâmetro topic=merchant_order ⇒ <font color='red'><b>Obrigatório</b></font>
            <br/>",
            Tags = new[] { "Pagamento" }
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReceberNotificacaoPagamento([FromQuery]long id, [FromQuery] string topic)
        {
            try
            {
                if (topic.Equals("merchant_order"))
                {                    
                    await _pagamentoService.ProcessarNotificacaoPagamento(id);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Tratar exceções, se necessário
                return StatusCode(500, $"Erro ao processar webhook: {ex.Message}");
            }
        }

        // Get : /pagamento/idPedido/qrcode
        [HttpGet("{idPedido}/qrcode")]
        [SwaggerOperation(
            Summary = "Endpoint para obter QRCode para pagamento de um pedido",
            Description = @"Obtém o QRCode para pagamento do pedido no Mercado Pago </br>                        
            <b>Parâmetros de entrada:</b>
            <br/> • <b>IdPedido</b>: o identificador do pedido relacionado ao pagamento. ⇒ <font color='red'><b>Obrigatório</b></font>
            <br/>",
            Tags = new[] { "Pagamento" }
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterQRCodePagamento(int idPedido)
        {
            try
            {                
                var qrCode = await _pagamentoService.ObterQRCodePagamento(idPedido);
                return Ok(qrCode);

            }
            catch (Exception ex)
            {
                // Tratar exceções, se necessário
                return StatusCode(500, $"Erro ao criar qrCode: {ex.Message}");
            }
        }
    }
}
