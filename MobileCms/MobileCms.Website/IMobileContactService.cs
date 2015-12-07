using System.Data;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileContactService
    {
        DataTable GetListByPage(string companyCode, string userName, string listType, int start, int limit, out int total);

        MobileContact GetMobileContactByOwnerUriAndListType(string ownerUri, string listType);

        void Create(MobileContact model);

        MobileContact GetById(string id);

        void Delete(string id);

        void Update(MobileContact model);

        MobileContact GetMobileContactByOwnerUriAndListFileName(string ownerUri, string listFileName);

        void SendMqttContactList(string ownerUri, string listFileName);
    }
}
