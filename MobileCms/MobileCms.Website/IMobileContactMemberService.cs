using System.Collections.Generic;
using System.Data;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileContactMemberService
    {
        DataTable GetListByPage(string companyCode, string displayName, string listFileName, string ownerUri, int start, int limit, out int total);

        List<MobileContactMember> GetMobileContactMembersByOwnerUriAndListFileName(string ownerUri, string listFileName);

        void Create(MobileContactMember model);

        MobileContactMember GetById(string id);

        void Delete(string id);

        void Update(MobileContactMember model);
    }
}
