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
    /// Summary description for editmobileuser
    /// </summary>
    public class editmobileuser : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            string id = RequestHelp.GetString("hidId");
            string displayName = RequestHelp.GetString("displayName");
            string mobilePhone = RequestHelp.GetString("mobilePhone");
            string gender = RequestHelp.GetString("gender");
            string birthday = RequestHelp.GetString("birthday");

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileUserExtensionService mobileUserExtensionService = container.Resolve<IMobileUserExtensionService>();

                MobileUserExtension tempMobileUserExtension = mobileUserExtensionService.GetMobileUserExtensionById(id);

                if (tempMobileUserExtension == null)
                {
                    jsonObject.Add("message", "用户数据异常");
                    context.Response.Write(jsonObject);
                    return;
                }

                tempMobileUserExtension.DisplayName = displayName;
                tempMobileUserExtension.MobilePhone = mobilePhone;
                tempMobileUserExtension.Gender = gender;
                tempMobileUserExtension.Birthday = birthday;
                tempMobileUserExtension.Updator = mobileUser.UserName;
                tempMobileUserExtension.UpdateTime = DateTime.Now;

                mobileUserExtensionService.Update(tempMobileUserExtension);

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