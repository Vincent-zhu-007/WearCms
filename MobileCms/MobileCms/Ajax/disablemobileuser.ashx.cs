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
    /// Summary description for disablemobileuser
    /// </summary>
    public class disablemobileuser : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            string id = RequestHelp.GetString("id");
            string status = RequestHelp.GetString("status");

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

                tempMobileUser.Status = status == "Y" ? "N" : "Y";

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