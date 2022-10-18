 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToDo;
using ToDo.Common.Exceptions;
using ToDo.Common.Exceptions.Logging;
using ToDo.Core.Managers.Interfaces;
using ToDo.ModelViews.ModelView;

namespace ToDo.Extenstions
{
    public static class ExceptionMiddlewareExtenstion
    {
        private static readonly HashSet<string> HandledExceptions = new()
        {
            typeof(ServiceValidationException).FullName,
            typeof(TazeezException).FullName,
            typeof(InvalidOperationExceptions).FullName
        };
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        try
                        {
                            await LogExceptionAsync(logger, context, contextFeature.Error).AnyContext();
                        }
                        catch (Exception)
                        {
                        }

                        var exception = contextFeature.Error;
                        var errorCode = 0;


                        var responseText = env.IsDevelopment() || env.IsEnvironment("Local")
                                            ? exception?.Message
                                            : "Something went wrong";
                        if (HandledExceptions.Contains(exception.GetType().FullName))
                        {
                            int statusCode = (int)HttpStatusCode.BadRequest;

                            if (exception is TazeezException ex)
                            {
                                statusCode = ex.ErrorCode == 406 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;
                                errorCode = ex.ErrorCode;
                            }
                            responseText = exception.Message;
                            context.Response.StatusCode = statusCode > 0 ? statusCode : (int)HttpStatusCode.BadRequest;


                        }

                        var error = new ErrorDetails()
                        {
                            Message = responseText,
                            ErrorCode = errorCode,
                            StatusCode = context.Response.StatusCode,
                        }.ToString();

                        await context.Response.WriteAsync(error, Encoding.UTF8).AnyContext();
                    }
                });
            });
        }

        private static async Task LogExceptionAsync(ILogger logger, HttpContext context, Exception exception)
        {
            var sb = new StringBuilder();

            var logMassage = new LogMessage
            {
                LogLevel = LogEventLevel.Error,
                ApplicationName = typeof(Startup).Namespace
            };

            if (HandledExceptions.Contains(exception.GetType().FullName))
            {
                logMassage.LogLevel = LogEventLevel.Information;
                if (exception.GetType().FullName.Equals(typeof(InvalidOperationExceptions).FullName))
                {
                    logMassage.LogLevel = LogEventLevel.Fatal;

                }
            }

            while (exception != null)
            {
                sb.Append($"{exception.Message}{Environment.NewLine}StackTrace:{exception.StackTrace}");
                exception = exception.InnerException;
            }

            logMassage.Message = sb.ToString();

            if (context != null)
            {
                logMassage.RequestPath = context.Request?.Path;

                var helperManager = context.RequestServices.GetService(typeof(ICommonManager)) as ICommonManager;

                var ClaimId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (!string.IsNullOrWhiteSpace(ClaimId) && int.TryParse(ClaimId, out int id))
                {
                    var user = helperManager.GetinfoUserFromDb(new UserModel { Id = id });

                    if (user != null)
                    {
                        logMassage.UserId = user.Id;
                        logMassage.UserEmail = user.Email;
                    }
                }
            }

            await logger.LogMessageAsync(logMassage).AnyContext();
        }
    }
}
