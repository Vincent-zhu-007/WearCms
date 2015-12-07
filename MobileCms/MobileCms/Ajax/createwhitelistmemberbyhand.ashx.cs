using System;
using System.Collections.Generic;
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
    /// Summary description for createwhitelistmemberbyhand
    /// </summary>
    public class createwhitelistmemberbyhand : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            String message = "";

            WeiXinUserExtension weiXinUserExtension = new MobileCms.Common.BasePageMember().GetWeiXinUserExtension();

            if (weiXinUserExtension != null)
            {
                string mobileContactId = RequestHelp.GetString("mobileContactId");
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
                IMobileContactService mobileContactService = container.Resolve<IMobileContactService>();

                MobileContact mobileContact = mobileContactService.GetById(mobileContactId);

                if (mobileContact != null)
                {
                    string ownerUri = mobileContact.OwnerUri;
                    string listFileName = mobileContact.ListFileName;

                    IMobileContactMemberService mobileContactMemberService = container.Resolve<IMobileContactMemberService>();

                    List<MobileContactMember> mobileContactMembers = mobileContactMemberService.GetMobileContactMembersByOwnerUriAndListFileName(ownerUri, listFileName);

                    if (mobileContactMembers != null && mobileContactMembers.Count >= 4)
                    {
                        jsonObject.Add("message", "最多只能建立4个成员，请直接对已有成员进行编辑");
                        context.Response.Write(jsonObject);
                        return;
                    }

                    MobileContactMember mobileContactMember = new MobileContactMember();
                    mobileContactMember.Id = Guid.NewGuid().ToString();
                    mobileContactMember.OwnerUri = ownerUri;
                    mobileContactMember.DisplayName = displayName;
                    mobileContactMember.MobilePhone = mobilePhone;
                    mobileContactMember.ShortNum = shortNum;
                    mobileContactMember.NumButton = numButton;
                    mobileContactMember.ListFileName = listFileName;
                    
                    mobileContactMemberService.Create(mobileContactMember);

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