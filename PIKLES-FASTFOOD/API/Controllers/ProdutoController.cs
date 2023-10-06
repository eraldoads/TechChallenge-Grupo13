using App.Application.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]

    [SwaggerResponse(204, "Requisição concluida, porém não há dados de retorno!")]
    [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido a sintaxe malformada!")]
    [SwaggerResponse(401, "Requisição requer autenticação do usuário!")]
    [SwaggerResponse(403, "Privilégios insuficientes!")]
    [SwaggerResponse(404, "O recurso solicitado não existe!")]
    [SwaggerResponse(412, "Condição prévia dada em um ou mais dos campos avaliado como falsa!")]
    [SwaggerResponse(500, "Servidor encontrou uma condição inesperada!")]

    public class ProdutoController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(ILogger<ProdutoController> logger)
        {
            _logger = logger;
        }

        public override bool Equals(object? obj)
        {
            return obj is ProdutoController controller &&
                   EqualityComparer<ILogger<ProdutoController>>.Default.Equals(_logger, controller._logger);
        }

        [HttpGet()]
        [SwaggerOperation(
        Summary = "Endpoint para listar todos os produtos cadastrados",
        Description = @"Endpoint para listar todos os produtos </br>
                      <b>Parâmetros de entrada:</b>
                       <br/> &bull; <b>NomeProduto</b>:busca exemplo nome Caso nenhum filtro seja informado, ele irá trazer todos &rArr; <font color='green'><b>Opcional</b></font>
                        ",
        Tags = new[] { "Produtos" }
        )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(List<Produto>))]
        [SwaggerResponse(206, "Conteúdo Parcial!", typeof(List<Produto>))]

        public async Task<IActionResult> Get([FromQuery] Produto filtroProdutos)
        {
            if (filtroProdutos is null)
            {
                throw new ArgumentNullException(nameof(filtroProdutos));
            }

            return Ok(new { Nome = "Hamburguer da Casa", Preco = 29.90, Categoria = "Lanche" });
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}