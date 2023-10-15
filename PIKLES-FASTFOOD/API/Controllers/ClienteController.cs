using Data.Context;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
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

    public class ClienteController : ControllerBase
    {
        private readonly MySQLContext _context;

        public ClienteController(MySQLContext context)
        {
            _context = context;
        }

        // GET: api/clientes
        [HttpGet()]
        [SwaggerOperation(
        Summary = "Endpoint para listar todos os clientes cadastrados",
        Description = @"Busca todos os Clientes</br>",
        Tags = new[] { "Clientes" }
        )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(List<Cliente>))]
        [SwaggerResponse(206, "Conteúdo Parcial!", typeof(List<Cliente>))]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            if (_context.Cliente == null)
                return NoContent();

            return await _context.Cliente.ToListAsync();
        }

        // GET: api/clientes/{id}
        [HttpGet("{id?}")]
        [SwaggerOperation(
        Summary = "Endpoint para listar um cliente específico pelo id",
        Description = @"Endpoint para listar um cliente específico pelo id </br>
                      <b>Parâmetros de entrada:</b>
                       <br/> • <b>id</b>: o identificador do cliente ⇒ <font color='red'><b>Obrigatório</b></font>
                        ",
        Tags = new[] { "Clientes" }
        )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(Cliente))]
        [SwaggerResponse(404, "Cliente não encontrado!", typeof(void))]
        public async Task<ActionResult<Cliente>> GetCliente(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NoContent();
            }
            return cliente;
        }

        // POST: api/clientes
        [HttpPost]
        [SwaggerOperation(
        Summary = "Endpoint para criar um novo cliente",
        Description = @"Endpoint para criar um novo cliente </br>
              <b>Parâmetros de entrada:</b>
                <br/> • <b>id</b>: o identificador do cliente a ser criado ⇒ <font color='green'><b>Opcional</b></font>
                <br/> • <b>nome</b>: o primeiro nome do cliente a ser criado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>sobrenome</b>: o sobrenome do cliente a ser criado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>cpf</b>: o CPF do cliente a ser criado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>email</b>: o e-mail do cliente a ser criado ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
        Tags = new[] { "Clientes" }
        )]
        [SwaggerResponse(201, "Cliente criado com sucesso!", typeof(Cliente))]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente cliente)
        {
            if (cliente == null)
                return BadRequest();

            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCliente", new { id = cliente.Id }, cliente);
        }

        // PATCH: api/clientes/{id}
        [HttpPatch("{id}")]
        [SwaggerOperation(
            Summary = "Endpoint para atualizar parcialmente um cliente pelo ID",
            Description = @"Endpoint para atualizar parcialmente um Cliente pelo ID </br>
              <b>Parâmetros de entrada:</b>
                <br/> • <b>ID</b>: o identificador do cliente. ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>operationType</b>: Este é um número que representa o tipo de operação a ser realizada. Os valores possíveis são 0 (Adicionar), 1 (Remover), 2 (Substituir), 3 (Mover), 4 (Copiar) e 5 (Testar). ⇒ <font color='green'><b>Opcional</b></font>
                <br/> • <b>path</b>: Este é o caminho do valor a ser alterado na estrutura de dados JSON. Por exemplo, se você tem um objeto com uma propriedade chamada ‘nomeProduto’, o caminho seria ‘/nomeProduto’. ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>op</b>: Esta é a operação a ser realizada. Os valores possíveis são ‘add’, ‘remove’, ‘replace’, ‘move’, ‘copy’ e ‘test. ⇒ <font color='green'><b>Opcional</b></font>
                <br/> • <b>from</b>: Este campo é usado apenas para as operações ‘move’ e ‘copy’. Ele especifica o caminho do local de onde o valor deve ser movido ou copiado. ⇒ <font color='green'><b>Opcional</b></font>
                <br/> • <b>value</b>:  Este é o valor a ser adicionado, substituído ou testado. ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Clientes" }
        )]
        [SwaggerResponse(204, "Cliente atualizado parcialmente com sucesso!", typeof(void))]
        public async Task<IActionResult> PatchCliente(int id, [FromBody] JsonPatchDocument<Cliente> patchDoc)
        {
            if (patchDoc != null)
            {
                var itemCliente = await _context.Cliente.FindAsync(id);

                if (itemCliente == null)
                    return NoContent();

                patchDoc.ApplyTo(itemCliente, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Entry(itemCliente).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(id))
                        return NoContent();
                    else
                        throw;
                }

                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/clientes/{id}
        [HttpPut("{id}")]
        [ValidateModel]
        [SwaggerOperation(
        Summary = "Endpoint para atualizar completamente um cliente pelo id",
        Description = @"Endpoint para atualizar completamente um cliente pelo id </br>
              <b>Parâmetros de entrada:</b>
                <br/> • <b>id</b>: o identificador do cliente a ser atualizado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>nome</b>: o primeiro nome do cliente a ser atualizado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>sobrenome</b>: o sobrenome do cliente a ser atualizado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>cpf</b>: o CPF do cliente a ser atualizado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>email</b>: o e-mail do cliente a ser atualizado ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
        Tags = new[] { "Clientes" }
        )]
        [SwaggerResponse(204, "Cliente atualizado com sucesso!", typeof(void))]
        public async Task<IActionResult> PutCliente(int id, [FromBody] Cliente cliente)
        {
            cliente.Id = id;

            if (cliente == null)
                return BadRequest();

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                    return Ok();
                else
                    throw;
            }

            return Ok();
        }

        // DELETE: api/clientes/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(
        Summary = "Endpoint para deletar um cliente pelo id",
        Description = @"Endpoint para deletar um cliente pelo id </br>
              <b>Parâmetros de entrada:</b>
               <br/> • <b>id</b>: o identificador do cliente a ser deletado ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
        Tags = new[] { "Clientes" }
        )]
        [SwaggerResponse(200, "Cliente deletado com sucesso!", typeof(Cliente))]
        public async Task<ActionResult<Cliente>> DeleteCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
                return NoContent();

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.Id == id);
        }
    }
}
