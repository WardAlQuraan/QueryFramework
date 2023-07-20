using BASES.BASE_REPO;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORIES.TEST_ENTITY
{
    public class TestEntityRepo : BaseRepo<TestEntity>
    {
        public TestEntityRepo(IDalSession dalSession) : base(dalSession)
        {
        }
    }
}
