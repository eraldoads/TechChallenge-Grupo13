using Domain.Base;
using Domain.Entities;
using Domain.EntitiesDTO;
using Domain.Port.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;
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

    public class ProdutoController : ControllerBase
    {
        private IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        // GET : /Produto
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
            List<Produto> produtos = await _produtoService.GetProdutos();
            return produtos;
        }

        // GET : /Produto/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Endpoint para listar um produto específico pelo id",
            Description = @"Endpoint para listar um produto específico pelo id </br>
                <b>Parâmetros de entrada:</b>
                <br/> • <b>id</b>: o identificador do produto ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Produtos" }
            )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(Produto))]
        public async Task<ActionResult<Produto?>> GetProduto(int id)
        {
            Produto? produto = await _produtoService.GetProdutoById(id);

            if (produto is null)
                return NoContent();

            return produto;
        }

        // GET : /Produto/categoria/{idCategoria?}
        [HttpGet("categoria/{idCategoria?}")]
        [SwaggerOperation(
            Summary = "Endpoint para listar os produtos de uma categoria específica",
            Description = @"Endpoint para listar os produtos de uma categoria específica </br>
                <b>Parâmetros de entrada:</b>
                <br/> • <b>idCategoria</b>: o id da categoria do produto ⇒ <font color='red'><b>Obrigatório</b></font>
                ",
            Tags = new[] { "Produtos" }
            )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(List<Categoria>))]
        public async Task<ActionResult<List<Categoria>>> GetProdutosPorIdCategoria(EnumCategoria? idCategoria)
        {
            if (idCategoria is null)
                return BadRequest();

            var produtos = await _produtoService.GetProdutosByIdCategoria(idCategoria);

            if (produtos is null || produtos.Count == 0)
                return NoContent();

            return produtos;
        }

        // POST : /produto
        [HttpPost]
        [SwaggerOperation(
            Summary = "Endpoint para criar um novo produto",
            Description = @"Endpoint para criar um novo produto </br>
                          <b>Parâmetros de entrada:</b>
                          <br/> • <b>nomeProduto</b>: o nome do produto a ser criado ⇒ <font color='red'><b>Obrigatório</b></font>
                          <br/> • <b>valorProduto</b>: o valor do produto a ser criado⇒ <font color='red'><b>Obrigatório</b></font>
                          <br/> • <b>IdCategoria</b>: a categoria do produto a ser criado, tem como definição os valores:⇒ <font color='red'><b>Obrigatório</b></font>
                              <br/>&nbsp;&emsp;&emsp;• <b>1</b> - Lanche
                              <br/>&nbsp;&emsp;&emsp;• <b>2</b> - Acompanhamento
                              <br/>&nbsp;&emsp;&emsp;• <b>3</b> - Bebida
                              <br/>&nbsp;&emsp;&emsp;• <b>4</b> - Sobremesa
                          <br/> • <b>descricaoProduto</b>: a descrição do produto a ser criado⇒ <font color='red'><b>Obrigatório</b></font>",
            Tags = new[] { "Produtos" }
        )]
        [SwaggerResponse(201, "Produto criado com sucesso!", typeof(Produto))]
        public async Task<ActionResult<Produto>> PostProduto([FromBody] ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest();

            var novoProduto = await _produtoService.PostProduto(produtoDTO);

            return CreatedAtAction("PostProduto", new { id = novoProduto.IdProduto }, novoProduto);
        }


        // PATCH : /produto/{id}
        [HttpPatch("{id}")]
        [SwaggerOperation(
            Summary = "Endpoint para atualizar parcialmente um produto pelo id",
            Description = @"Endpoint para atualizar parcialmente um produto pelo id </br>
                          <b>Parâmetros de entrada:</b>
                          <br/> • <b>ID</b>: o identificador do produto. ⇒ <font color='red'><b>Obrigatório</b></font>
                          <br/> • <b>operationType</b>: Este é um número que representa o tipo de operação a ser realizada. Os valores possíveis são 0 (Adicionar), 1 (Remover), 2 (Substituir), 3 (Mover), 4 (Copiar) e 5 (Testar). ⇒ <font color='green'><b>Opcional</b></font>
                          <br/> • <b>path</b>: Este é o caminho do valor a ser alterado na estrutura de dados JSON. Por exemplo, se você tem um objeto com uma propriedade chamada ‘nomeProduto’, o path seria ' ""path"": ""nomeProduto"" '. ⇒ <font color='red'><b>Obrigatório</b></font>
                          <br/> • <b>op</b>: Esta é a operação a ser realizada. Os valores possíveis são ‘add’, ‘remove’, ‘replace’, ‘move’, ‘copy’ e ‘test. ⇒ <font color='green'><b>Opcional</b></font>
                          <br/> • <b>from</b>: Este campo é usado apenas para as operações ‘move’ e ‘copy’. Ele especifica o caminho do local de onde o valor deve ser movido ou copiado. ⇒ <font color='green'><b>Opcional</b></font>
                          <br/> • <b>value</b>:  Este é o valor a ser adicionado, substituído ou testado. ⇒ <font color='red'><b>Obrigatório</b></font>
                          ",
            Tags = new[] { "Produtos" }
        )]
        [SwaggerResponse(204, "Produto atualizado parcialmente com sucesso!", typeof(void))]
        public async Task<IActionResult> PatchProduto(int id, [FromBody] JsonPatchDocument<Produto> patchDoc)
        {
            try
            {
                await _produtoService.PatchProduto(id, patchDoc);
                return NoContent();
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
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

        // PUT : /produto/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Endpoint para atualizar completamente um produto pelo id",
            Description = @"Endpoint para atualizar completamente um produto pelo id </br>
              <b>Parâmetros de entrada:</b>
                <br/> • <b>id</b>: o identificador do produto a ser atualizado ⇒ <font color='red'><b>Obrigatório</b></font>
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
        [SwaggerResponse(240, "Produto atualizado com sucesso!", typeof(void))]
        public async Task<IActionResult> PutProduto(int id, [FromBody] Produto produtoInput)
        {
            try
            {
                await _produtoService.PutProduto(id, produtoInput);
                return NoContent();
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
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

        // DELETE : /produto/{id}
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
            try
            {
                var deletedProduto = await _produtoService.DeleteProduto(id);

                if (deletedProduto is null)
                    return NoContent();

                return deletedProduto;
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { id, error = "Produto não encontrado" });
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