
using System.Collections.Generic;
using MobileCms.Data;
namespace MobileCms.Website
{
    public interface IWeiXinInMobileService
    {
        string CreateWeiXinInMobileFromServer(string openId, string mobileUserName, string numButton, string companyCode);

        List<WeiXinInMobile> GetWeiXinInMobileByWeiXinOwnerUri(string weiXinOwnerUri);
    }
}
