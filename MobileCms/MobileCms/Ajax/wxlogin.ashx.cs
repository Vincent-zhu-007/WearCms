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
    /// Summary description for wxlogin
    /// </summary>
    public class wxlogin : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObjectResult = new JObject();

            string openId = RequestHelp.GetString("openId");
            string mobilePhone = RequestHelp.GetString("mobilePhone");
            string password = RequestHelp.GetString("password");

            string publicId = ConfigHelp.GetConfigString("WeiXinPublicId_BDXTS");
            string userName = mobilePhone + "@" + publicId;
            
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IWeiXinUserService weiXinUserService = container.Resolve<IWeiXinUserService>();

            WeiXinUserExtension model = weiXinUserService.GetWeiXinUserExtensionByUserName(userName);

            if (model == null)
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "手机号码错误");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            string md5Password = DEncryptHelp.Encrypt(password, 1).ToLower();
            if (!model.Password.Equals(md5Password))
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "密码错误");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            context.Session.Add("WeiXinUserId", model.Id);
            context.Session.Add("WeiXinUserName", model.UserName);

            CookieHelp.SetCookie("WeiXinUserName", userName);
            CookieHelp.SetDESEncryptedCookie("WeiXinUserPwd", password);

            jsonObjectResult.Add("result", "1");
            jsonObjectResult.Add("data", "200");
            context.Response.Write(jsonObjectResult.ToString());
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