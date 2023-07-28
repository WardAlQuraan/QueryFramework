using BASES.BASE_REPO;
using BASES.BASE_SERVICE;
using CONNECTION_FACTORY.DAL_SESSION;
using ENTITIES.FAMILY;
using REPOSITORIES.FAMILY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES.FAMILY.FAMILY_MEMBER
{
    public class FamilyMemberService : BaseService<FamilyMember, long>, IFamilyMemberService
    {
        FamilyRepo _familyRepo;
        FamilyMembereRepo _repo;
        public FamilyMemberService(IDalSession dalSession) : base(dalSession)
        {

        }

        public override async Task<long> InsertAsync(FamilyMember entity)
        {
            try
            {
                dalSession.Begin();
                _familyRepo = new FamilyRepo(dalSession);
                var family = await _familyRepo.GetAsync(entity.FamilyId);
                if(family is null)
                    throw new Exception($"Invalid Family id : {entity.FamilyId}");
                _repo = new FamilyMembereRepo(dalSession);
                var parent = await _repo.GetByCondition(new { FamilyId = entity.FamilyId, IsFamilyParent = 1 });
                if(parent is null)
                {
                    entity.ParentId = null;
                    entity.IsFamilyParent = 1;
                }
                else
                {
                    entity.ParentId = parent.Id;
                    entity.IsFamilyParent = 2;
                }
                var res = await _repo.InsertAsync(entity);

                dalSession.Commit();
                return res;
            }
            catch
            {
                dalSession.Rollback();
                throw;
            }
        }

    }
}
