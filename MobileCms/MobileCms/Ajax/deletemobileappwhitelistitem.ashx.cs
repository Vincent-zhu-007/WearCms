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
    /// Summary description for deletemobileappwhitelistitem
    /// </summary>
    public class deletemobileappwhitelistitem : IHttpHandler, IRequiresSessionState
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
                IMobileAppItemService mobileAppItemService = container.Resolve<IMobileAppItemService>();

                MobileAppItem tempMobileAppItem = mobileAppItemService.GetById(id);

                if (tempMobileAppItem == null)
                {
                    jsonObject.Add("message", "用户数据异常");
                    context.Response.Write(jsonObject);
                    return;
                }

                string ownerUri = tempMobileAppItem.OwnerUri;

                mobileAppItemService.Delete(id);

                IMobileAppService mobileAppService = container.Resolve<IMobileAppService>();
                MobileApp mobileApp = mobileAppService.GetMobileAppByOwnerUriAndListFileName(tempMobileAppItem.OwnerUri, tempMobileAppItem.ListFileName);

                mobileApp.Etag = Guid.NewGuid().ToString();
                mobileApp.UpdateTime = DateTime.Now;
                mobileApp.Updator = mobileUser.OwnerUri;
                mobileAppService.Update(mobileApp);

                IRpcService rpcService = container.Resolve<IRpcService>();
                rpcService.runCmdUpdateApp(ownerUri, mobileUser.CompanyCode);
                
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