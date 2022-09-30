using Shared.Exceptions;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApp.Extentions;

namespace WebApp.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
        {
            try
            {
                await next(context);
            }
            catch (HhcmException exception)
            {
                var response = AppApiResponse<object>.Failed(exception.FriendlyMessage, exception.HttpStatusCode);
                await HandleExceptionAsync(context, response, exception, logger);
            }
            catch (Exception exception)
            {
                //var response = HhcmApiResponse<object>.Failed(
                //    $"Something went wrong. To help us resolve the issue please keep the 'Reference Id' handy. Reference Id: {Activity.Current?.Id}",
                //    HttpStatusCode.InternalServerError);
                //await HandleExceptionAsync(context, response, exception, logger);
                logger.LogError($"Activity Id: {Activity.Current?.Id}");
                logger.LogError(exception.UnwrapExceptionMessages());
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"<div class=\"container\"><h1 style=\"text-align: center;font-size:15em; margin-bottom:0px\">500</h1><h1 style=\"text-align: center;\">Something went wrong.</h1><h5 style=\"text-align: center;\">Reference Id: {Activity.Current?.Id}</h5></div>");

            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context, AppApiResponse<object> response, Exception exception, ILogger<ExceptionHandlingMiddleware> logger)
        {


            logger.LogError("****************************** API ERROR ******************************");
            logger.LogError($"Activity Id: {Activity.Current?.Id}");
            logger.LogError(exception.UnwrapExceptionMessages());

            var result =
                JsonSerializer.Serialize(
                    response,
                    MatchStyleOfExistingMvcProblemDetails());

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            return context.Response.WriteAsync(result);

            static JsonSerializerOptions MatchStyleOfExistingMvcProblemDetails()
            {
                return new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never
                };
            }
        }
    }
}
