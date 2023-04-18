using LBT_Api.Exceptions;

namespace LBT_Api.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            // Custom exceptions
            catch (BadRequestException exception)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(exception.Message);
            }
            catch (DatabaseOperationFailedException exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(exception.Message);
            }
            catch (InvalidModelException exception)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(exception.Message);
            }
            catch (NotFoundException exception)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(exception.Message);
            }
            // Built-in exceptions
            catch (ArgumentNullException exception)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(exception.Message);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(exception.Message);
            }
        }
    }
}
