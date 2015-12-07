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
    /// Summary description for modifypassword
    /// </summary>
    public class modifypassword : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            string oldPassword = RequestHelp.GetString("oldPassword");
            string newPassword = RequestHelp.GetString("newPassword");
            string rePassword = RequestHelp.GetString("rePassword");

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                if (!newPassword.Equals(rePassword))
                {
                    jsonObject.Add("message", "新密码与确认密码不一致");
                    context.Response.Write(jsonObject);
                    return;
                }

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileUserService mobileUserService = container.Resolve<IMobileUserService>();

                MobileUser tempMobileUser = mobileUserService.GetMobileUserByUserName(mobileUser.UserName);

                string oldMd5Password = DEncryptHelp.Encrypt(oldPassword, 1).ToLower();
                if (!tempMobileUser.Password.Equals(oldMd5Password))
                {
                    jsonObject.Add("message", "原密码错误");
                    context.Response.Write(jsonObject);
                    return;
                }

                string newMd5Password = DEncryptHelp.Encrypt(newPassword, 1).ToLower();
                tempMobileUser.Password = newMd5Password;
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