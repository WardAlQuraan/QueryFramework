using BASES.BASE_SERVICE;
using ENTITIES;
using FAMILY_TREE.BASE_CONTROLLER;

namespace FAMILY_TREE.Controllers
{
    public class SoftDataController : BaseController<SoftData>
    {
        public SoftDataController(IBaseService<SoftData> baseService) : base(baseService)
        {
        }
    }
}
