using System.Collections.Generic;
using MobileCms.Data;
using Newtonsoft.Json.Linq;

namespace MobileCms.Website
{
    public interface IMobileOrgStructureService
    {
        List<MobileOrgStructure> GetAllMobileOrgStructureByStatus();

        void BuildCreateMobileContactMemberTree(List<MobileOrgStructure> mobileOrgStructures, List<MobileOrgStructure> allMobileOrgStructures, List<MobileUserExtension> allMobileUserExtensions, JArray jsonArray);
    }
}
