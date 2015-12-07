using System.Web;
using System.Web.SessionState;
using MobileCms.Common;
using Newtonsoft.Json.Linq;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for logout
    /// </summary>
    public class logout : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            context.Session.Add("UserId", "");
            context.Session.Add("UserName", "");
            context.Session.Add("User", "");

            CookieHelp.SetCookie("UserName", "");
            CookieHelp.SetCookie("UserPwd", "");

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