using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClienteBff.Config.Exceptions
{
    public class HttpResponseExceptionFilter : ActionFilterAttribute
    {

        //TODO deixar ambos com o mesmo tipo de resposta para o front
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorResponse = new ResponseData() { Successful = false, Message = "Erro de validação de campos", codError = 4 };
                var errorsInModelState = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage).ToArray());
                foreach (var error in errorsInModelState)
                {
                    foreach (var subError in error.Value)
                    {
                        var errorModel = new ErrorModel
                        {
                            FieldName = error.Key,
                            Message = subError
                        };
                        errorResponse.Error.Add(errorModel);
                    }
                }
                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException)
            {
                var httpResponseException = (HttpResponseException)context.Exception;
                switch (httpResponseException.StatusCode)
                {
                    case 404:
                        context.Result = new NotFoundObjectResult(httpResponseException.Value);
                        break;
                    case 400:
                        context.Result = new BadRequestObjectResult(httpResponseException.Value);
                        break;
                    default:
                        context.Result = new BadRequestObjectResult(httpResponseException.Value);
                        break;
                }
                
                context.ExceptionHandled = true;
            }
        }
    }
}
