using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for getmobileappconfightmloptions
    /// </summary>
    public class getmobileappconfightmloptions : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileAppConfigService mobileAppConfigService = container.Resolve<IMobileAppConfigService>();

                Dictionary<string, string> mobileAppConfigMap = mobileAppConfigService.GetMobileAppConfigCacheFromServer(mobileUser.CompanyCode);

                string result = "";

                foreach (string key in mobileAppConfigMap.Keys)
                {
                    result += "<option value='" + key + "'>" + mobileAppConfigMap[key] + "</option>";
                }

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