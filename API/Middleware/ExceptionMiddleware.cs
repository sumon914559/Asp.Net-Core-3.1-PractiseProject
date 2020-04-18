using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Utility.Exceptions;
using Utility.Helpers;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        // IMyScopedService is injected into Invoke
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HendelMyallException(e, context);
            }
           
        }

        private async Task HendelMyallException(Exception exception, HttpContext context)
        {
            var code = HttpStatusCode.InternalServerError;
            var error = new ErrorResponse();
            error.statusCode = (int)code;

            if (_env.IsDevelopment())
            {
                error.DeveloperMessage = exception.StackTrace;
            }
            else
            {
                error.Message = exception.Message;
            }
            

            switch (exception)
            {
                case MyApplicationExceptions myApplicationExceptions:
                    {
                        error.statusCode = (int)HttpStatusCode.NotFound;
                        break;
                    }
                case UnauthorizedAccessException unauthorizedAccessException:
                    {
                         error.statusCode = (int)HttpStatusCode.Unauthorized;
                         error.Message = "You are not Authorized";
                         break;
                    }
                default:
                    {
                        break;
                    }
            }

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "applicaton/json";
            context.Response.StatusCode = error.statusCode;
            await context.Response.WriteAsync(result);


        }
    }
}
