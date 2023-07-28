using BASES.BASE_SERVICE;
using COMMON.ENUMS;
using ENTITIES.FAMILY;
using FAMILY_TREE.ATTRIBUTES;
using FAMILY_TREE.BASE_CONTROLLER;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERVICES.FAMILY.FAMILY_MEMBER;

namespace FAMILY_TREE.Controllers
{
    [FamilyAuthorize(AuthRole.ADMIN)]
    public class FamilyMemberController : BaseController<FamilyMember, long>
    {
        IFamilyMemberService _service;
        public FamilyMemberController(IFamilyMemberService service) : base(service)
        {
        }
    }
}
