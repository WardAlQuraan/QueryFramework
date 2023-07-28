using BASES.BASE_REPO;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES;
using ENTITIES.USER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORIES.USER
{
    public class UserRepo : BaseRepo<User,int>
    {
        public UserRepo(IDalSession dalSession) : base(dalSession)
        {
        }
    }
}
