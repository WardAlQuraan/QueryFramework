using COMMON.API_RESPONSE;

namespace FAMILY_TREE.MIDDLEWARES.EXCEPTION
{
    public interface IExceptionHandler
    {
        Task<ApiRespone> HandleExceptionAsync(Exception exception);
    }
}
