using System.Collections.Generic;
using System.Linq;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;

namespace MobileCms.WebsiteService
{
    public class MobileOrgStructureService : IMobileOrgStructureService
    {
        public List<MobileOrgStructure> GetAllMobileOrgStructureByStatus()
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.Code.OfType<MobileOrgStructure>().Where(m => m.Status == "Y").ToList();
            }
        }

        public void BuildCreateMobileContactMemberTree(List<MobileOrgStructure> mobileOrgStructures, List<MobileOrgStructure> allMobileOrgStructures, List<MobileUserExtension> allMobileUserExtensions, JArray jsonArray)
        {
            if (mobileOrgStructures != null && mobileOrgStructures.Count > 0)
            {
                foreach (MobileOrgStructure mobileOrgStructure in mobileOrgStructures)
                {
                    List<MobileUserExtension> mobileUserExtensions = allMobileUserExtensions.Where(m => m != null && m.OrgStructure == mobileOrgStructure.CodeName).ToList();

                    JObject jsonObject = new JObject();
                    jsonObject.Add("id", mobileOrgStructure.CodeName);
                    jsonObject.Add("text", mobileOrgStructure.Description);

                    if (mobileOrgStructure.LevelNum == 1)
                    {
                        jsonObject.Add("parent", "#");
                    }
                    else
                    {
                        jsonObject.Add("parent", mobileOrgStructure.ParentCode);
                    }

                    //加人
                    if (mobileUserExtensions != null && mobileUserExtensions.Count > 0)
                    {
                        foreach (MobileUserExtension mobileUserExtension in mobileUserExtensions)
                        {
                            JObject jsonObjectMobileUser = new JObject();
                            jsonObjectMobileUser.Add("id", mobileUserExtension.OwnerUri);
                            jsonObjectMobileUser.Add("text", mobileUserExtension.MobilePhone + " | " + mobileUserExtension.DisplayName);
                            jsonObjectMobileUser.Add("parent", mobileOrgStructure.CodeName);

                            jsonArray.Add(jsonObjectMobileUser);
                        }
                    }

                    List<MobileOrgStructure> childMobileOrgStructures = allMobileOrgStructures.Where(m => m.ParentCode == mobileOrgStructure.CodeName).ToList();

                    if (childMobileOrgStructures != null && childMobileOrgStructures.Count > 0)
                    {
                        BuildCreateMobileContactMemberTree(childMobileOrgStructures, allMobileOrgStructures, allMobileUserExtensions, jsonArray);
                    }

                    jsonArray.Add(jsonObject);
                }
            }
        }
    }
}
