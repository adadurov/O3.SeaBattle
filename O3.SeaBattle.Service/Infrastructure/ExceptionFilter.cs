using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace O3.SeaBattle.Service.Infrastructure
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var status = HttpStatusCode.InternalServerError;

            if ( context.Exception is InvalidOperationException)
            {
                status = HttpStatusCode.BadRequest;
            }
            else if (context.Exception is ArgumentException)
            {
                status = HttpStatusCode.BadRequest;
            }

            context.Result = new ObjectResult( new {
                detail = context.Exception.Message,
                status = status
            })
            {
                StatusCode = (int)status
            };
        }
    }
}
