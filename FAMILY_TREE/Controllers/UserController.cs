using COMMON.ENUMS;
using ENTITIES.USER;
using FAMILY_TREE.ATTRIBUTES;
using FAMILY_TREE.BASE_CONTROLLER;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SERVICES.TEST_ENTITY;

namespace FAMILY_TREE.Controllers
{
    [FamilyAuthorize(AuthRole.ADMIN)]
    public class UserController : BaseController<User,int>
    {
        private IUserService _service;
        public UserController(IUserService service) : base(service)
        {
            _service = service;
        }
    }
}
