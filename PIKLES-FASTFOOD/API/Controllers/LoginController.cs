using Data.Context;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

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

    public class LoginController : ControllerBase
    {
        private readonly MySQLContext _context;

        public LoginController(MySQLContext context)
        {
            _context = context;
        }

        // POST: /Login
        [HttpPost]
        [SwaggerOperation(
            Summary = "Endpoint para fazer login do cliente via CPF",
            Description = @"Endpoint para fazer login do cliente via CPF </br>
                <b>Parâmetros de entrada:</b>
                    <br/> • <b>cpf</b>: o CPF do cliente - Somente números. ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Login" }
        )]
        [SwaggerResponse(200, "Cliente encontrado com sucesso!", typeof(Cliente))]
        [SwaggerResponse(404, "Cliente não encontrado")]
        public async Task<ActionResult<Cliente>> LoginCliente([FromBody] Login model)
        {
            try
            {
                if (_context != null && _context.Cliente != null)
                {
                    var cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.CPF == model.CPF);

                    if (cliente == null)
                        return NotFound("Cliente não encontrado");

                    return cliente;
                }
                else
                {
                    // Lidar com a situação em que _context.Cliente é nulo
                    return StatusCode(500, "Ocorreu um erro interno no servidor. Entre em contato com o suporte técnico.");
                }
            }
            catch (Exception ex)
            {
                // Lidar com a exceção aqui
                return StatusCode(500, $"Ocorreu um erro: {ex.Message}");
            }
        }

    }
}
