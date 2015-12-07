using System;
using System.Web;
using System.Web.SessionState;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for editmobileappconfig
    /// </summary>
    public class editmobileappconfig : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            string id = RequestHelp.GetString("hidId");
            string description = RequestHelp.GetString("description");
            string packageName = RequestHelp.GetString("packageName");
            
            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileAppConfigService mobileAppConfigService = container.Resolve<IMobileAppConfigService>();

                MobileAppConfig mobileAppConfig = mobileAppConfigService.GetById(id);

                if (mobileAppConfig == null)
                {
                    jsonObject.Add("message", "用户数据异常");
                    context.Response.Write(jsonObject);
                    return;
                }

                mobileAppConfig.Description = description;
                mobileAppConfig.PackageName = packageName;
                mobileAppConfig.Updator = mobileUser.UserName;
                mobileAppConfig.UpdateTime = DateTime.Now;

                mobileAppConfigService.Update(mobileAppConfig);

                jsonObject.Add("message", "200");
                context.Response.Write(jsonObject);
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