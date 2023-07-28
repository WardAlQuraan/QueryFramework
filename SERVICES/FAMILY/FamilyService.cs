using BASES.BASE_SERVICE;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES.FAMILY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.FAMILY
{
    public class FamilyService : BaseService<Family, long>, IFamilyService
    {
        public FamilyService(IDalSession dalSession) : base(dalSession)
        {
        }
    }
}
