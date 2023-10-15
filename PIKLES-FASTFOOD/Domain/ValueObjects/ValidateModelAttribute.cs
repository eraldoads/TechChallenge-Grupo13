using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Domain.ValueObjects
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
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
