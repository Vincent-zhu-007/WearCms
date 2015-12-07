using System.Collections.Generic;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileUserExtensionService
    {
        MobileUserExtension GetMobileUserExtensionById(string id);

        MobileUserExtension GetMobileUserExtensionByUserName(string userName);

        MobileUserExtension GetMobileUserExtensionByOwnerUri(string ownerUri);

        void Update(MobileUserExtension model);

        List<MobileUserExtension> GetMobileUserExtensionByCompanyCode(string companyCode);

        MobileUserExtension GetMobileUserExtensionByOwnerUriUnStatus(string ownerUri);
    }
}
