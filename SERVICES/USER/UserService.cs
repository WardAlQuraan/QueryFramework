using BASES.BASE_REPO;
using BASES.BASE_SERVICE;
using COMMON.HELPER;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES;
using ENTITIES.USER;
using REPOSITORIES.ROLE;
using REPOSITORIES.USER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.TEST_ENTITY
{
    public class UserService : BaseService<User,int>, IUserService
    {
        private UserRepo _repo;
        public UserService(IDalSession dalSession) : base(dalSession)
        {
        }

        public override async Task<int> InsertAsync(User entity)
        {
            try
            {
                dalSession.Begin();
                if (string.IsNullOrEmpty(entity.Password))
                {
                    throw new Exception("Password is required");
                }
                _repo = new UserRepo(dalSession);

                var userId = await _repo.InsertAsync(entity);

                var userPassword = new UserPassword()
                {
                    IsDeleted = 0,
                    Password = PasswordHashser.HashPassword(entity.Password),
                    UserId = userId
                };
                await _repo.Generic.InsertAsync<UserPassword,int>(userPassword);
                dalSession.Commit();
                return userId;
            }
            catch (Exception ex)
            {
                dalSession.Rollback();
                throw;
            }
        }
    }
}
