using BASES.BASE_SERVICE;
using COMMON.ENUMS;
using ENTITIES.FAMILY;
using FAMILY_TREE.ATTRIBUTES;
using FAMILY_TREE.BASE_CONTROLLER;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERVICES.FAMILY;

namespace FAMILY_TREE.Controllers
{
    [FamilyAuthorize(AuthRole.ADMIN)]
    public class FamilyController : BaseController<Family, long>
    {
        private readonly IFamilyService _service;
        public FamilyController(IFamilyService service) : base(service)
        {
        }
        
    }
}
