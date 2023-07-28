using BASES.BASE_SERVICE;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES.ROLE;
using REPOSITORIES.ROLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.ROLE
{
    public class RoleService : BaseService<Role,string>,IRoleService
    {
        private RoleRepo _repo;
        public RoleService(IDalSession dalSession) : base(dalSession)
        {
        }
    }
}
