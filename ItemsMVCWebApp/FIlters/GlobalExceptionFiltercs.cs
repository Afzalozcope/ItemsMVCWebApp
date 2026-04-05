using System;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ItemsMVCWebApp.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Debug.WriteLine("Exception: " + context.Exception.Message);
            Debug.WriteLine("StackTrace: " + context.Exception.StackTrace);
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string message = "Something went wrong. Please try again.";

            // Database errors
            if (context.Exception is DbUpdateException)
            {
                status = HttpStatusCode.BadRequest;
                message = "Database update failed.";
            }

            // Argument errors (bad input)
            else if (context.Exception is ArgumentException)
            {
                status = HttpStatusCode.BadRequest;
                message = context.Exception.Message;
            }

            //// Unauthorized access
            //else if (context.Exception is UnauthorizedAccessException)
            //{
            //    status = HttpStatusCode.Unauthorized;
            //    message = "You are not authorized.";
            //}

            var response = new
            {
                success = false,
                error = message
            };

            context.Response = context.Request.CreateResponse(status, response);
        }
    }
}