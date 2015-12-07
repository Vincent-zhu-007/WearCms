using System.Data;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileAppService
    {
        DataTable GetListByPage(string companyCode, string userName, string listType, int start, int limit, out int total);

        MobileApp GetMobileAppByOwnerUriAndListType(string ownerUri, string listType);

        void Create(MobileApp model);

        MobileApp GetById(string id);

        void Delete(string id);

        void Update(MobileApp model);

        MobileApp GetMobileAppByOwnerUriAndListFileName(string ownerUri, string listFileName);
    }
}
