using FAMILY_TREE.BASE_CONTROLLER;
using ENTITIES.ROLE;
using SERVICES.ROLE;
using FAMILY_TREE.ATTRIBUTES;
using COMMON.ENUMS;

namespace FAMILY_TREE.Controllers
{
    [FamilyAuthorize(AuthRole.ADMIN)]
    public class RoleController : BaseController<Role,string>
    {
        private readonly IRoleService _service;
        public RoleController(IRoleService service):base (service)
        {
        }


    }
}