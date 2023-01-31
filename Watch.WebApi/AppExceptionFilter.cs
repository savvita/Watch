using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Watch.WebApi.Exceptions;

namespace Watch.WebApi
{
    public class AppExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is InvalidCredentialsException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "invalid-credentials"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                }
                else if (context.Exception is ConflictException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "login-is-registered"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Conflict
                    };
                }
                else if (context.Exception is AuthorizationException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "authorization-failed"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }
                else if (context.Exception is InternalServerException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "internal-server-error"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }
                else
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "internal-server-error"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }


                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
