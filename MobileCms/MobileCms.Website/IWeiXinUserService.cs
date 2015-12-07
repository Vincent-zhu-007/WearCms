using System.Data;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IWeiXinUserService
    {
        DataTable GetWeiXinUsersByPage(string companyCode, string userName, int start, int limit, out int total);

        WeiXinUser GetWeiXinUserById(string id);

        WeiXinUserExtension GetWeiXinUserExtensionById(string id);

        WeiXinUserExtension GetWeiXinUserExtensionByUserName(string userName);

        WeiXinUser GetWeiXinUserByOwnerUri(string ownerUri);

        WeiXinUserExtension GetWeiXinUserExtensionByUserNameAndPassword(string userName, string password);

        WeiXinUser GetWeiXinUserByOpenId(string openId);

        void Create(WeiXinUserExtension model);

        string SendMobileListeningFromServer(string openId, string companyCode);
    }
}
