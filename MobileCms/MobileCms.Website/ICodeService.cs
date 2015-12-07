using System.Collections.Generic;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface ICodeService
    {
        List<Code> GetCodeCache(string companyCode);

        Dictionary<string, string> GetCodeCacheByCategoryFromServer(string category, string companyCode);
    }
}
