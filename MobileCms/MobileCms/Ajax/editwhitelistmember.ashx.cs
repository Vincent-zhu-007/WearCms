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
    /// Summary description for editwhitelistmember
    /// </summary>
    public class editwhitelistmember : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            String message = "";

            WeiXinUserExtension weiXinUserExtension = new MobileCms.Common.BasePageMember().GetWeiXinUserExtension();

            if (weiXinUserExtension != null)
            {
                string mobileContactMemberId = RequestHelp.GetString("mobileContactMemberId");
                string displayName = RequestHelp.GetString("displayName");
                string mobilePhone = RequestHelp.GetString("mobilePhone");
                string shortNum = RequestHelp.GetString("shortNum") == null ? "" : RequestHelp.GetString("shortNum");
                string numButton = RequestHelp.GetString("numButton");

                if (String.IsNullOrEmpty(numButton))
                {
                    jsonObject.Add("message", "请选择按键");
                    context.Response.Write(jsonObject);
                    return;
                }

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileContactMemberService mobileContactMemberService = container.Resolve<IMobileContactMemberService>();

                MobileContactMember mobileContactMember = mobileContactMemberService.GetById(mobileContactMemberId);

                if (mobileContactMember != null)
                {
                    mobileContactMember.DisplayName = displayName;
                    mobileContactMember.MobilePhone = mobilePhone;
                    mobileContactMember.ShortNum = shortNum;
                    mobileContactMember.NumButton = numButton;
                    
                    mobileContactMemberService.Update(mobileContactMember);

                    string ownerUri = mobileContactMember.OwnerUri;
                    string listFileName = mobileContactMember.ListFileName;
                    IMobileContactService mobileContactService = container.Resolve<IMobileContactService>();
                    MobileContact mobileContact = mobileContactService.GetMobileContactByOwnerUriAndListFileName(ownerUri, listFileName);

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