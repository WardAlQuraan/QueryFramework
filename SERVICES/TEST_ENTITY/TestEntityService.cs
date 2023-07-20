using BASES.BASE_SERVICE;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.TEST_ENTITY
{
    public class TestEntityService : BaseService<TestEntity>, ITestEntityService
    {
        public TestEntityService(IDalSession dalSession) : base(dalSession)
        {
        }
    }
}
