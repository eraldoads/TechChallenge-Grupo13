using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Domain.ValueObjects
{
    public class AjustaDataHoraLocal : ActionFilterAttribute
    {
        /// <summary>
        /// Este método é chamado antes da execução de uma ação e verifica se o estado do model é válido.
        /// </summary>
        /// <param name="context">O contexto da execução da ação.</param>
        /// <remarks>
        /// Se o estado do model não for válido, este método define o resultado do contexto como um objeto 
        /// que contém os detalhes do problema de validação e um código de status HTTP 412 - Precondition Failed.
        /// </remarks>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ObjectResult(new ValidationProblemDetails(context.ModelState))
                {
                    StatusCode = (int)HttpStatusCode.PreconditionFailed // Alteração do código de status para 412 - Precondition Failed
                };
            }
        }

    }
}
