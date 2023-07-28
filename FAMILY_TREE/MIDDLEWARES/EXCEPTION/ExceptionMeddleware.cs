using COMMON.API_RESPONSE;
using COMMON.EXCEPTION_RESPONSE;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace FAMILY_TREE.MIDDLEWARES.EXCEPTION
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async Task<ApiRespone> HandleExceptionAsync(Exception exception)
        {
            var exceptionRespone = new ApiRespone()
            {
                ErrorMessage = exception.Message,
                IsError = true
            };

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(FamilyValidationException))
                    exceptionRespone.StatusCode = 400;
            else
                exceptionRespone.StatusCode = 500;


            await ValueTask.CompletedTask;
            return exceptionRespone;
        }
    }
}
