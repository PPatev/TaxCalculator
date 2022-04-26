using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using TaxCalculator.Infrastructure.Exceptions;

namespace TaxCalculator.Infrastructure.Filters
{
    public class DuplicateRequestKeyExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }
        public void OnActionExecuted(ActionExecutedContext context) 
        {
            if (context.Exception is DuplicateRequestKeyException exception)
            {
                ObjectResult result = new ObjectResult(exception.Message);
                result.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Result = result;
                context.ExceptionHandled = true;
            }
        }
    }
}
