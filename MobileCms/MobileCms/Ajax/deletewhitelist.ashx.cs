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
    /// Summary description for deletewhitelist
    /// </summary>
    public class deletewhitelist : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            string id = RequestHelp.GetString("id");

            WeiXinUserExtension weiXinUserExtension = new MobileCms.Common.BasePageMember().GetWeiXinUserExtension();

            if (weiXinUserExtension != null)
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileContactMemberService mobileContactMemberService = container.Resolve<IMobileContactMemberService>();

                MobileContactMember tempMobileContactMember = mobileContactMemberService.GetById(id);

                if (tempMobileContactMember == null)
                {
                    jsonObject.Add("message", "用户数据异常");
                    context.Response.Write(jsonObject);
                    return;
                }

                string ownerUri = tempMobileContactMember.OwnerUri;
                string listFileName = tempMobileContactMember.ListFileName;

                mobileContactMemberService.Delete(id);

                IMobileContactService mobileContactService = container.Resolve<IMobileContactService>();
                MobileContact mobileContact = mobileContactService.GetMobileContactByOwnerUriAndListFileName(ownerUri, tempMobileContactMember.ListFileName);

                mobileContact.Etag = Guid.NewGuid().ToString();
                mobileContact.UpdateTime = DateTime.Now;
                mobileContact.Updator = weiXinUserExtension.OwnerUri;
                mobileContactService.Update(mobileContact);

                mobileContactService.SendMqttContactList(ownerUri, listFileName);
                
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