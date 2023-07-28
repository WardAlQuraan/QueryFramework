using FAMILY_TREE.MIDDLEWARES.EXCEPTION;
using Newtonsoft.Json;

namespace FAMILY_TREE.EXTENSIONS
{
    public static class ExceptionHandlerExtension
    {
        public static void UseIExceptionHandler(this IApplicationBuilder app , string errorHandlingPath)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception e)
                {
                    var handler = app.ApplicationServices.GetService<IExceptionHandler>();
                    if (handler is not null)
                    {
                        var res = await handler.HandleExceptionAsync(e);
                        context.Response.StatusCode = res.StatusCode;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(res));
                    }
                }
            });
        }
    }
}
