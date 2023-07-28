using BASES.BASE_REPO;
using COMMON;
using COMMON.AUTHORIZATION;
using COMMON.HELPER;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES.USER;
using ENTITIES.VIEW_MODELS;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.AUTH
{
    public class AuthService:IAuthService
    {
        public IDalSession _dal;
        public AuthService(IDalSession dal) 
        {
            _dal= dal;
        }
        
        public async Task<TokenAuthorization> Login(AuthVM authVM)
        {
            _dal.Begin();
            try
            {
                var repo = new Repository(_dal);
                var query = $@"Select U.USERNAME as Username , UP.Password , UP.USER_ID as UserId from [{typeof(User).GetTableName()}] U , [{typeof(UserPassword).GetTableName()}] UP 
                                where 1=1
                                and U.USERNAME = @Username 
                                and U.ID = UP.USER_ID 
                                and UP.IS_DELETED = 0";

                var res = await repo.GetByQuery<AuthVM>(query, authVM);
                if (res is null)
                    throw new Exception("Invalid Username");
                if(!PasswordHashser.VerifyPassword(res.Password, authVM.Password))
                {
                    throw new Exception("Invalid Password");
                }

                query = $@"Select U.USERNAME as Username , U.Role_code as Role , U.ID as UserId from [{typeof(User).GetTableName()}] U 
                                where 1=1
                                and U.ID = @UserId ";
                var authorizationRes = await repo.GetByQuery<TokenAuthorization>(query, new { UserId = res.UserId});
                authorizationRes.Token = GenerateJwtToken(authorizationRes);
                _dal.Dispose();
                return authorizationRes;

            }catch(Exception ex)
            {
                _dal.Dispose();
                throw; 
            }
        }

        public string GenerateJwtToken(TokenAuthorization authorization)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalApp.AppSettings.JWT.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Username",authorization.Username),
                new Claim("Role",authorization.Role),
                new Claim("UserId",authorization.UserId.ToString())
            };
            var token = new JwtSecurityToken(GlobalApp.AppSettings.JWT.Issuer,
                GlobalApp.AppSettings.JWT.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
