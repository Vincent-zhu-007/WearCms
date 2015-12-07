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
    /// Summary description for login
    /// </summary>
    public class login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            string userName = RequestHelp.GetString("username");
            string password = RequestHelp.GetString("password");

            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IWeiXinUserService weiXinUserService = container.Resolve<IWeiXinUserService>();

            WeiXinUserExtension model = weiXinUserService.GetWeiXinUserExtensionByUserName(userName);

            if (model == null)
            {
                jsonObject.Add("message", "用户名错误");
                context.Response.Write(jsonObject);
                return;
            }

            string md5Password = DEncryptHelp.Encrypt(password, 1).ToLower();
            if (!model.Password.Equals(md5Password))
            {
                jsonObject.Add("message", "密码错误");
                context.Response.Write(jsonObject);
                return;
            }

            if (String.IsNullOrEmpty(model.WebSiteRole))
            {
                jsonObject.Add("message", "平台未授权用户,请与系统管理员联系.");
                context.Response.Write(jsonObject);
                return;
            }

            context.Session.Add("UserId", model.Id);
            context.Session.Add("UserName", model.UserName);

            CookieHelp.SetCookie("UserName", userName);
            CookieHelp.SetDESEncryptedCookie("UserPwd", password);

            jsonObject.Add("message", "200");
            context.Response.Write(jsonObject);
            return;
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