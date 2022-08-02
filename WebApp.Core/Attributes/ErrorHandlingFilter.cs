using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Core.Attributes
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            // var exception = context.Exception;

            context.ExceptionHandled = true; //optional 
        }
    }
}
