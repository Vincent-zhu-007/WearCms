using System.Collections.Generic;
using System.Data;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileAppItemService
    {
        DataTable GetListByPage(string description, string listFileName, int start, int limit, out int total);

        void Create(MobileAppItem model);

        MobileAppItem GetById(string id);

        void Delete(string id);

        List<MobileAppItem> GetMobileAppItemByOwnerUriAndListFileName(string ownerUri, string listFileName);

        List<MobileAppItem> GetMobileAppItemsByAppCodeName(string appCodeName);
    }
}
