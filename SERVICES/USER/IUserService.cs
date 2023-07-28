using BASES.BASE_SERVICE;
using ENTITIES;
using ENTITIES.USER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.TEST_ENTITY
{
    public interface IUserService:IBaseService<User,int>
    {
    }
}
