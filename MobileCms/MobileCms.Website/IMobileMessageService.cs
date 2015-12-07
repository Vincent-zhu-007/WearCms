using System.Data;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileMessageService
    {
        DataTable GetListByPage(string companyCode, string userName, int start, int limit, out int total);

        MobileMessage GetById(string id);

        string CreateMobileMessageFromServer(string companyCode, string ownerUri, string targetUris, string messageContent);

        string EditMobileMessageFromServer(string companyCode, string ownerUri, string id, string targetUris, string messageContent);

        string SendMobileMessageFromServer(string companyCode, string id);

        string DeleteMobileMessageFromServer(string companyCode, string ownerUri, string id);
    }
}
