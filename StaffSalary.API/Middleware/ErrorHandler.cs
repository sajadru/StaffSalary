using Microsoft.Data.SqlClient;
using StaffSalary.API.Middleware.Model;
using StaffSalary.Infrastructure.UnitOfWork;
using System.Net;

namespace StaffSalary.API.Middleware
{
    public class ErrorHandler
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<UnitOfWork> _logger;
        public ErrorHandler(RequestDelegate next, ILogger<UnitOfWork> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                var errorTitle = string.Empty;
                try
                {
                    errorTitle = ex.StackTrace.Split("at")[1];
                }
                catch
                {
                    errorTitle = "Error";
                }
                _logger.LogWarning(ex, errorTitle);
                await HandleUnauthorizeExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                var errorTitle = string.Empty;
                var errorName = "Unexpected Error";
                try
                {
                    errorTitle = ex.StackTrace.Split("at")[1];
                }
                catch
                {
                    errorTitle = "Error";
                }

                _logger.LogError(ex, errorTitle);
                await HandleExceptionAsync(context, new Exception(errorName));
            }

        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            }.ToString());
        }

        private async Task HandleUnauthorizeExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            }.ToString());
        }
    }
}
