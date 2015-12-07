using System.Collections.Generic;
using System.Data;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileAppConfigService
    {
        DataTable GetListByPage(string companyCode, string description, int start, int limit, out int total);

        MobileAppConfig GetById(string id);

        void Create(MobileAppConfig model);

        void Update(MobileAppConfig model);

        void Delete(string id);

        MobileAppConfig GetMobileAppConfigByCodeName(string codeName);

        Dictionary<string, string> GetMobileAppConfigCacheFromServer(string companyCode);

        string ClearMobileAppConfigCacheFromServer(string companyCode);
    }
}
