using System.Data;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileUserFileService
    {
        DataTable GetListByPage(string companyCode, string userName, int start, int limit, out int total);

        MobileUserFile GetById(string id);
    }
}
