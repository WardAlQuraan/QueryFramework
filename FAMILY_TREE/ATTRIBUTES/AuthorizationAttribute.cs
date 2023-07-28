using COMMON.ENUMS;
using ENTITIES.ROLE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SERVICES.AUTH;
using ENTITIES.USER;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using COMMON.API_RESPONSE;

namespace FAMILY_TREE.ATTRIBUTES
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FamilyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<AuthRole> _roles;
        private readonly IAuthService _authService;
        private readonly JsonResult errorResult = new JsonResult(new ApiRespone() { ErrorMessage = "Unauthorized", IsError = true, StatusCode = 401 });

        public FamilyAuthorizeAttribute(params AuthRole[] roles)
        {
            _roles = roles ?? new AuthRole[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Substring(7);

            if(token is not null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadJwtToken(token);
                // authorization
                if (decodedToken.Payload.TryGetValue("Role", out object role)) {
                    if(_roles.Any())
                    {
                        if (role == null || !_roles.Contains(Enum.Parse<AuthRole>(role.ToString())))
                        {
                            context.HttpContext.Response.StatusCode = 401;
                            context.Result = errorResult;
                        }

                    }
                }
                else if (_roles.Any() && role is null)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = errorResult;
                }

            }
            else
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = errorResult;
            }

        }
    }
}
