using App.Application.Interfaces;
using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using static Domain.ValueObjects.ResultBase;
using System.Diagnostics;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    [Produces("application/json", new string[] { })]
    [SwaggerResponse(204, "Requisição concluída, porém não há dados de retorno!", null)]
    [SwaggerResponse(400, "A solicitação não pode ser entendida pelo servidor devido a sintaxe malformada!", null)]
    [SwaggerResponse(401, "Requisição requer autenticação do usuário!", null)]
    [SwaggerResponse(403, "Privilégios insuficientes!", null)]
    [SwaggerResponse(404, "O recurso solicitado não existe!", null)]
    [SwaggerResponse(412, "Condição prévia dada em um ou mais dos campos avaliado como falsa!", null)]
    [SwaggerResponse(500, "Servidor encontrou uma condição inesperada!", null)]
    [Consumes("application/json", new string[] { })]

    public class ClienteController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(ILogger<ClienteController> logger)
        {
            _logger = logger;
        }

        public override bool Equals(object? obj)
        {
            return obj is ClienteController controller &&
                   EqualityComparer<ILogger<ClienteController>>.Default.Equals(_logger, controller._logger);
        }

        //private readonly IClienteAppService _appService;

        //// Usando a injeção de construtor
        //public ClienteController(IClienteAppService appService) : base("Pikle-FastFood")
        //{
        //    _appService = appService;
        //}


        [HttpGet()]
        [SwaggerOperation(
        Summary = "Endpoint para listar todos os clientes cadastrados",
        Description = @"Endpoint para listar todos os Clientes </br>
                      <b>Parâmetros de entrada:</b>
                       <br/> &bull; <b>Nome</b>:busca exemplo nome Caso nenhum filtro seja informado, ele irá trazer todos &rArr; <font color='green'><b>Opcional</b></font>
                        ",
        Tags = new[] { "Clientes" }
        )]
        [SwaggerResponse(200, "Consulta executada com sucesso!", typeof(List<Cliente>))]
        [SwaggerResponse(206, "Conteúdo Parcial!", typeof(List<Cliente>))]

        public async Task<IActionResult> Get([FromQuery] GetCliente filtroCliente)
        {
            if (filtroCliente is null)
            {
                throw new ArgumentNullException(nameof(filtroCliente));
            }
            try
            {
                //var result = await _appService.Get(filtroCliente);

                // Lista de nomes, cpfs e emails para criar os itens
                var nomes = new[] { "Cliente esfomeado", "Cliente satisfeito", "Cliente exigente", "Cliente fiel", "Cliente indeciso", "Cliente simpático", "Cliente apressado", "Cliente curioso", "Cliente generoso", "Cliente educado" };
                var cpfs = new[] { "12345678910", "10987654321", "13579246810", "24680135792", "14725836901", "15975346820", "35715924680", "95135724680", "75395124680", "85296374101" };
                var emails = new[] { "cliente@esfomeado.com.br", "cliente@satisfeito.com.br", "cliente@exigente.com.br", "cliente@fiel.com.br", "cliente@indeciso.com.br", "cliente@simpatico.com.br", "cliente@apressado.com.br", "cliente@curioso.com.br", "cliente@generoso.com.br", "cliente@educado.com.br" };

                // Cria a variável resultArray vazio com o tamanho da lista de nomes
                var resultArray = new object[nomes.Length];

                // Loop foreach para percorrer a lista de nomes e criar os itens
                int index = 0; // Cria uma variável para armazenar o índice do resultArray
                foreach (var nome in nomes)
                {
                    // Crie um objeto anônimo com as propriedades Nome, CPF e email, usando os valores da lista
                    var item = new { Nome = nome, CPF = cpfs[index], email = emails[index] };
                    // Adicione o item ao resultArray na posição index
                    resultArray[index] = item;
                    // Incremente o index em 1
                    index++;
                }

                // Verifica se o resultArray está nulo.
                if (resultArray == null)
                {
                    return NotFound(); // retorna um NotFoundResult com o status 404
                }
                return Ok(resultArray); // retorna um OkObjectResult com o status 200 e o objeto result no corpo da resposta
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); // retorna um ObjectResult com o status 500 e um objeto ProblemDetails no corpo da resposta
            }
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    #region [Tratamento do Status de Retorno | Criar uma classe especifica dentro do projeto]
    public abstract class ControllerBase : Controller
    {
        public readonly Stopwatch _stopwatch;

        protected string _systemName { get; set; }

        protected ControllerBase(string systemName)
        {
            _stopwatch = new Stopwatch();
            _systemName = systemName;
        }

        public virtual IActionResult FromResult(ModelStateDictionary modelState)
        {
            List<ResultError> listaErros = modelState.Keys.SelectMany((string key) => modelState[key]!.Errors.Select((ModelError x) => new ResultError
            {
                CampoErro = key,
                MensagemErro = x.ErrorMessage
            })).ToList();
            PreConditionResult<ErrorValidacao> result = new PreConditionResult<ErrorValidacao>(new ErrorValidacao
            {
                ListaErros = listaErros
            });
            return FromResult(result);
        }

        public virtual IActionResult FromResult<T>(PagedResult<T> pagedResult)
        {
            return PagedResult(pagedResult);
        }

        private IActionResult PagedResult<T>(PagedResult<T> pagedResult)
        {
            if (pagedResult.ResultType == ResultType.NoContent)
            {
                return FromResult(new NoContentResult<T>());
            }

            SetPaginationHeader(pagedResult._Offset, pagedResult._Limit, pagedResult._Total);
            if (pagedResult.ResultType == ResultType.PartialContent)
            {
                return FromResult(new PartialContentResult<IList<T>>(pagedResult.Data));
            }

            return FromResult(new SuccessResult<IList<T>>(pagedResult.Data));
        }

        private static T GetPropertyValue<T>(object o, string propertyName)
        {
            return (T)o.GetType().GetProperty(propertyName)!.GetValue(o, null);
        }

        public virtual IActionResult FromResult<T>(Result<T> result)
        {
            try
            {
                if (result.GetType().Name.Contains("PagedResult"))
                {
                    int propertyValue = GetPropertyValue<int>(result, "_Offset");
                    int propertyValue2 = GetPropertyValue<int>(result, "_Limit");
                    int propertyValue3 = GetPropertyValue<int>(result, "_Total");
                    if (result.ResultType == ResultType.PartialContent)
                    {
                        SetPaginationHeader(propertyValue, propertyValue2, propertyValue3);
                    }
                }
            }
            catch
            {
            }

            switch (result.ResultType)
            {
                case ResultType.Accepted:
                    return ReturnStatusCode(202, result);
                case ResultType.BadRequest:
                    return ReturnStatusCode(400, result);
                case ResultType.Created:
                    return ReturnStatusCode(201, result);
                case ResultType.Exception:
                    {
                        int num = result.Code ?? 500;
                        return ReturnStatusCode(num, result);
                    }
                case ResultType.NoContent:
                    return ReturnStatusCode(204);
                case ResultType.NotFound:
                    return ReturnStatusCode(404);
                case ResultType.PartialContent:
                    return ReturnStatusCode(206, result);
                case ResultType.PermissionDenied:
                    return ReturnStatusCode(403, result);
                case ResultType.PreCondition:
                    return ReturnStatusCode(412, result);
                case ResultType.Success:
                    return ReturnStatusCode(200, result);
                case ResultType.Unauthorized:
                    return ReturnStatusCode(401, result);
                default:
                    return ReturnStatusCode(result.Code ?? 501, result);
            }
        }

        private IActionResult ReturnStatusCode(int httpStatusCode)
        {
            return ReturnStatusCode<object>(httpStatusCode);
        }

        private IActionResult ReturnStatusCode<T>(int httpStatusCode, Result<T> result = null)
        {
            if (httpStatusCode == 412 && result != null && result.Error != null)
            {
                ErrorValidacao error = result.Error;
                if ((error != null && error.ListaErros?.Count > 0) || !string.IsNullOrEmpty(result.Error?.MensagemErro))
                {
                    return StatusCode(httpStatusCode, result.Error);
                }
            }

            if (result == null || result.Data == null)
            {
                return StatusCode(httpStatusCode);
            }

            return StatusCode(httpStatusCode, result.Data);
        }

        public virtual void SetPaginationHeader(int _Offset, int _Limit, int _Total)
        {
            base.Request.HttpContext.Response.Headers.Add("_offset", _Offset.ToString());
            base.Request.HttpContext.Response.Headers.Add("_limit", _Limit.ToString());
            base.Request.HttpContext.Response.Headers.Add("_total", _Total.ToString());
        }

        public class SuccessResult<T> : Result<T>
        {
            public override ResultType ResultType => ResultType.Success;

            public SuccessResult(T data)
            {
                base.Data = data;
            }
        }

        public class PartialContentResult<T> : Result<T>
        {
            public override ResultType ResultType => ResultType.PartialContent;

            public PartialContentResult(T data)
            {
                base.Data = data;
            }
        }

        public class NoContentResult<T> : Result<T>
        {
            public override ResultType ResultType => ResultType.NoContent;

            public NoContentResult()
            {
            }
        }

        public class PreConditionResult<T> : Result<T>
        {
            public override ResultType ResultType => ResultType.PreCondition;

            public PreConditionResult(ErrorValidacao error)
            {
                if (error != null)
                {
                    base.Error = error;
                }
            }
        }
        #endregion
    
    }
}
