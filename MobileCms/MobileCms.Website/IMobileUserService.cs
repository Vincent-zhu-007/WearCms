using System.Collections.Generic;
using System.Data;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileUserService
    {
        DataTable GetMobileUsersByPage(string companyCode, string userName, int start, int limit, out int total);

        MobileUser GetMobileUserById(string id);

        MobileUser GetMobileUserByUserName(string userName);

        MobileUser GetMobileUserByOwnerUri(string ownerUri);

        List<MobileUser> GetMobileUserByOrgStructure(string orgStructure);

        MobileUser GetMobileUserByUserNameAndPassword(string userName, string password);

        void Create(MobileUser model);

        void Update(MobileUser model);

        void Delete(string id);

        List<MobileUser> GetMobileUserByCompanyCode(string companyCode);

        string LockMobileFromServer(string companyCode, string id);

        string ClearMobileFromServer(string companyCode, string id);

        Dictionary<string, string> GetMobileUserDicByCompanyCode(string companyCode);
    }
}
