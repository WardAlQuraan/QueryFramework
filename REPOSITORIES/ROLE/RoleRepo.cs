using BASES.BASE_REPO;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES.ROLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORIES.ROLE
{
    public class RoleRepo : BaseRepo<Role, string>
    {
        public RoleRepo(IDalSession dalSession) : base(dalSession)
        {
        }
    }
}
