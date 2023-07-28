using BASES.BASE_REPO;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES.FAMILY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORIES.FAMILY
{
    public class FamilyMembereRepo : BaseRepo<FamilyMember, long>
    {
        public FamilyMembereRepo(IDalSession dalSession) : base(dalSession)
        {
        }
    }
}
