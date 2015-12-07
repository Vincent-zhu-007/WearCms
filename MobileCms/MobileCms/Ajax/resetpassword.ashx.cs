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
    /// Summary description for resetpassword
    /// </summary>
    public class resetpassword : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            string id = RequestHelp.GetString("id");

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileUserService mobileUserService = container.Resolve<IMobileUserService>();

                MobileUser tempMobileUser = mobileUserService.GetMobileUserById(id);

                if (tempMobileUser == null)
                {
                    jsonObject.Add("message", "用户数据异常");
                    context.Response.Write(jsonObject);
                    return;
                }

                string password = "123456";
                string md5Password = DEncryptHelp.Encrypt(password, 1).ToLower();

                tempMobileUser.Password = md5Password;
                tempMobileUser.Updator = mobileUser.UserName;
                tempMobileUser.UpdateTime = DateTime.Now;

                mobileUserService.Update(tempMobileUser);

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