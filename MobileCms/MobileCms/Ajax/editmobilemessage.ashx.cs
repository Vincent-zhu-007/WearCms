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
    /// Summary description for editmobilemessage
    /// </summary>
    public class editmobilemessage : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            String message = "";

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                string companyCode = mobileUser.CompanyCode;

                string ownerUri = mobileUser.OwnerUri;
                string id = RequestHelp.GetString("hidId");
                string targetUris = RequestHelp.GetString("targetUris");
                string content = RequestHelp.GetString("content");

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileMessageService mobileMessageService = container.Resolve<IMobileMessageService>();

                string result = mobileMessageService.EditMobileMessageFromServer(companyCode, ownerUri, id, targetUris, content);

                jsonObject.Add("message", result);
                context.Response.Write(jsonObject);
                return;
            }

            jsonObject.Add("message", message);
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