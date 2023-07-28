using COMMON.AUTHORIZATION;
using ENTITIES.VIEW_MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.AUTH
{
    public interface IAuthService
    {
        Task<TokenAuthorization> Login(AuthVM authVM);
    }
}
