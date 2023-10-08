using Data.Context;
using Domain.Base;
using Domain.Entities;
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

    public class ProdutoController : ControllerBase
    {
        private readonly MySQLContext _context;

        public ProdutoController(MySQLContext context)
        {
            _context = context;
        }

        // GET : /api/Produto
        [HttpGet()]
        [SwaggerOperation(
        Summary = "Endpoint para listar todos os produtos cadastrados",
        Description = @"Busca todos os produtos </br>",
        Tags = new[] { "Produtos" }
        )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(List<Produto>))]
        [SwaggerResponse(206, "Conteúdo Parcial!", typeof(List<Produto>))]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            if (_context.Produto == null)
                return NotFound();

            return await _context.Produto.ToListAsync();
        }

        // GET : /api/Produto/{id}
        [HttpGet("{id?}")]
        [SwaggerOperation(
            Summary = "Endpoint para listar um produto específico pelo id",
            Description = @"Endpoint para listar um produto específico pelo id </br>
                <b>Parâmetros de entrada:</b>
                <br/> • <b>id</b>: o identificador do produto ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Produtos" }
            )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(Produto))]
        public async Task<ActionResult<Produto>> GetProduto(int? id)
        {
            if (id == null)
                return BadRequest();

            var produto = await _context.Produto.FindAsync(id);

            if (produto == null)
                return NotFound();

            return produto;
        }

        // GET : /api/Produto/categoria/{idCategoria?}
        [HttpGet("categoria/{idCategoria?}")]
        [SwaggerOperation(
            Summary = "Endpoint para listar os produtos de uma categoria específica",
            Description = @"Endpoint para listar os produtos de uma categoria específica </br>
                <b>Parâmetros de entrada:</b>
                <br/> • <b>idCategoria</b>: o id da categoria do produto ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Produtos" }
            )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(List<Produto>))]
        public async Task<ActionResult<List<Produto>>> GetProdutosPorIdCategoria(EnumCategoria? idCategoria)
        {
            if (idCategoria == null)
                return BadRequest();

            var produtos = await _context.Produto.Where(x => x.IdCategoria == idCategoria).ToListAsync();

            if (produtos == null || produtos.Count == 0)
                return NotFound();

            return produtos;
        }

        // POST : /api/produto
        [HttpPost]
        [SwaggerOperation(
        Summary = "Endpoint para criar um novo produto",
        Description = @"Endpoint para criar um novo produto </br>
                      <b>Parâmetros de entrada:</b>
                        <br/> • <b>id</b>: o identificador do produto a ser criado ⇒ <font color='green'><b>Opcional</b></font>
                        <br/> • <b>codigoProduto</b>: o código do produto a ser criado ⇒ <font color='red'><b>Obrigatório</b></font>
                        <br/> • <b>nomeProduto</b>: o nome do produto a ser criado ⇒ <font color='red'><b>Obrigatório</b></font>
                        <br/> • <b>valorProduto</b>: o valor do produto a ser criado⇒ <font color='red'><b>Obrigatório</b></font>
                        <br/> • <b>categoriaProduto</b>: a categoria do produto a ser criado, tem como definição os valores:⇒ <font color='red'><b>Obrigatório</b></font>
                            <br/>&nbsp;&emsp;&emsp;• <b>1</b> - Lanche
                            <br/>&nbsp;&emsp;&emsp;• <b>2</b> - Acompanhamento
                            <br/>&nbsp;&emsp;&emsp;• <b>3</b> - Bebida
                            <br/>&nbsp;&emsp;&emsp;• <b>4</b> - Sobremesa
                        <br/> • <b>descricaoProduto</b>: a descrição do produto a ser  criado⇒ <font color='red'><b>Obrigatório</b></font>
                        ",
        Tags = new[] { "Produtos" }
        )]
        [SwaggerResponse(201, "Produto criado com sucesso!", typeof(Produto))]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            if (produto == null)
                return BadRequest();

            _context.Produto.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduto", new { id = produto.Id }, produto);
        }

        // PATCH : /api/produto/{id}
        [HttpPatch("{id}")]
        [SwaggerOperation(
                Summary = "Endpoint para atualizar parcialmente um produto pelo id",
                Description = @"Endpoint para atualizar parcialmente um produto pelo id </br>
              <b>Parâmetros de entrada:</b>
                <br/> • <b>ID</b>: o identificador do cliente. ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>operationType</b>: Este é um número que representa o tipo de operação a ser realizada. Os valores possíveis são 0 (Adicionar), 1 (Remover), 2 (Substituir), 3 (Mover), 4 (Copiar) e 5 (Testar). ⇒ <font color='green'><b>Opcional</b></font>
                <br/> • <b>path</b>: Este é o caminho do valor a ser alterado na estrutura de dados JSON. Por exemplo, se você tem um objeto com uma propriedade chamada ‘nomeProduto’, o caminho seria ‘/nomeProduto’. ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>op</b>: Esta é a operação a ser realizada. Os valores possíveis são ‘add’, ‘remove’, ‘replace’, ‘move’, ‘copy’ e ‘test. ⇒ <font color='green'><b>Opcional</b></font>
                <br/> • <b>from</b>: Este campo é usado apenas para as operações ‘move’ e ‘copy’. Ele especifica o caminho do local de onde o valor deve ser movido ou copiado. ⇒ <font color='green'><b>Opcional</b></font>
                <br/> • <b>value</b>:  Este é o valor a ser adicionado, substituído ou testado. ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
                Tags = new[] { "Produtos" }
                )]
        [SwaggerResponse(204, "Cliente atualizado parcialmente com sucesso!", typeof(void))]
        public async Task<IActionResult> PatchProduto(int id, [FromBody] JsonPatchDocument<Produto> patchDoc)
        {
            if (patchDoc != null)
            {
                var itemProduto = await _context.Produto.FindAsync(id);
                if (itemProduto == null)
                    return NotFound();

                patchDoc.ApplyTo(itemProduto, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Entry(itemProduto).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(id))
                        return NotFound();
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

        // PUT : /api/produto/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Endpoint para atualizar completamente um produto pelo id",
            Description = @"Endpoint para atualizar completamente um produto pelo id </br>
              <b>Parâmetros de entrada:</b>
                <br/> • <b>id</b>: o identificador do produto a ser atualizado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>codigoProduto</b>: o código do produto a ser atualizado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>nomeProduto</b>: o nome do produto a ser atualizado ⇒ <font color='red'><b>Obrigatório</b></font>
                <br/> • <b>valorProduto</b>: o valor do produto a ser atualizado⇒ <font color='red'><b>Obrigatório</b></font>
                        <br/> • <b>categoriaProduto</b>: a categoria do produto a ser criado, tem como definição os valores:⇒ <font color='red'><b>Obrigatório</b></font>
                            <br/>&nbsp;&emsp;&emsp;• <b>1</b> - Lanche
                            <br/>&nbsp;&emsp;&emsp;• <b>2</b> - Acompanhamento
                            <br/>&nbsp;&emsp;&emsp;• <b>3</b> - Bebida
                            <br/>&nbsp;&emsp;&emsp;• <b>4</b> - Sobremesa
                <br/> • <b>descricaoProduto</b>: a descrição do produto a ser  atualizado⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Produtos" }
            )]
        [SwaggerResponse(204, "Cliente atualizado com sucesso!", typeof(void))]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            produto.Id = id;

            if (produto == null)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE : /api/produto/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Endpoint para deletar um produto pelo id",
            Description = @"Endpoint para deletar um produto pelo id </br>
              <b>Parâmetros de entrada:</b>
               <br/> • <b>id</b>: o identificador do produto a ser deletado ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Produtos" }
            )]
        [SwaggerResponse(200, "Produto deletado com sucesso!", typeof(Produto))]
        public async Task<ActionResult<Produto>> DeleteProduto(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
                return NotFound();

            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.Id == id);
        }
    }
}