using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for getremoveandaddmobileappconfightmloptions
    /// </summary>
    public class getremoveandaddmobileappconfightmloptions : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            
            string result = "";

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                string id = RequestHelp.GetString("id");

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileAppService mobileAppService = container.Resolve<IMobileAppService>();

                MobileApp mobileApp = mobileAppService.GetById(id);

                string ownerUri = mobileApp.OwnerUri;
                string listFileName = mobileApp.ListFileName;

                IMobileAppConfigService mobileAppConfigService = container.Resolve<IMobileAppConfigService>();
                Dictionary<string, string> mobileAppConfigMap = mobileAppConfigService.GetMobileAppConfigCacheFromServer(mobileUser.CompanyCode);

                IMobileAppItemService mobileAppItemService = container.Resolve<IMobileAppItemService>();
                List<MobileAppItem> mobileAppItems = mobileAppItemService.GetMobileAppItemByOwnerUriAndListFileName(ownerUri, listFileName);

                string listboxAddOptions = "";
                if (mobileAppItems != null && mobileAppItems.Count > 0)
                {
                    foreach (MobileAppItem mobileAppItem in mobileAppItems)
                    {

                        string description = "";
                        if (mobileAppConfigMap.ContainsKey(mobileAppItem.AppCodeName))
                        {
                            description = mobileAppConfigMap[mobileAppItem.AppCodeName].ToString();

                            listboxAddOptions += "<option value='" + mobileAppItem.AppCodeName + "'>" + description + "</option>";
                        }
                        else
                        {
                            mobileAppConfigMap.Remove(mobileAppItem.AppCodeName);
                            continue;
                        }

                        mobileAppConfigMap.Remove(mobileAppItem.AppCodeName);
                    }
                }

                string listboxRemoveOptions = "";
                foreach (string key in mobileAppConfigMap.Keys)
                {
                    listboxRemoveOptions += "<option value='" + key + "'>" + mobileAppConfigMap[key] + "</option>";
                }

                result = listboxAddOptions + "&" + listboxRemoveOptions;

                context.Response.Write(result);
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}