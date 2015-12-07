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
    /// Summary description for createmobileappconfig
    /// </summary>
    public class createmobileappconfig : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            string codeName = RequestHelp.GetString("codeName");
            string description = RequestHelp.GetString("description");
            string packageName = RequestHelp.GetString("packageName");
            
            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileAppConfigService mobileAppConfigService = container.Resolve<IMobileAppConfigService>();

                MobileAppConfig tempMobileAppConfig = mobileAppConfigService.GetMobileAppConfigByCodeName(codeName);

                if (tempMobileAppConfig != null)
                {
                    jsonObject.Add("message", "编码已经存在");
                    context.Response.Write(jsonObject);
                    return;
                }

                MobileAppConfig model = new MobileAppConfig();
                model.Id = Guid.NewGuid().ToString();
                model.CodeName = codeName;
                model.Description = description;
                model.PackageName = packageName;
                model.FileUrl = "";
                model.Status = "Y";
                model.CreateTime = DateTime.Now;
                model.Creator = mobileUser.UserName;

                mobileAppConfigService.Create(model);

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