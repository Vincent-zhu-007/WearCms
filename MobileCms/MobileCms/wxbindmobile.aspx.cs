using System;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MobileCms
{
    public partial class wxbindmobile : BasePageWeiXinMember
    {
        public string openId = "";
        
        public string errorMessage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["WeiXinAuthorization"] != null && Session["WeiXinAuthorization"].ToString() != "")
            {
                WeiXinAuthorization weiXinAuthorization = Session["WeiXinAuthorization"] as WeiXinAuthorization;

                openId = weiXinAuthorization.OpenId;
            }

            if (!String.IsNullOrEmpty(openId))
            {
                JObject jsonObject = new JObject();

                WeiXinUserExtension weiXinUserExtension = GetWeiXinUserExtension();

                if (weiXinUserExtension != null)
                {
                    IComponentContainer container = ComponentContainerFactory.CreateContainer();

                    string mobileUserName = RequestHelp.GetString("no");

                    IMobileUserService mobileUserService = container.Resolve<IMobileUserService>();
                    MobileUser mobileUser = mobileUserService.GetMobileUserByUserName(mobileUserName);

                    if (mobileUser == null)
                    {
                        errorMessage = "二维码中的设备号码不存在，请与客服联系。";
                        return;
                    }

                    string numButton = "";

                    string mobileOwnerUri = mobileUser.OwnerUri;

                    //sip:000100000000014@sip.nuowei.com_WhiteList
                    string mobilelistFileName = mobileOwnerUri + "_WhiteList";

                    IMobileContactMemberService mobileContactMemberService = container.Resolve<IMobileContactMemberService>();
                    List<MobileContactMember> mobileContactMembers = mobileContactMemberService.GetMobileContactMembersByOwnerUriAndListFileName(mobileOwnerUri, mobilelistFileName);

                    if (mobileContactMembers != null && mobileContactMembers.Count > 0)
                    {
                        if (mobileContactMembers.Count >= 4)
                        {
                            numButton = "1";
                        }
                        else
                        {
                            numButton = (mobileContactMembers.Count + 1).ToString();
                        }
                    }
                    else
                    {
                        numButton = "1";
                    }

                    IWeiXinInMobileService weiXinInMobileService = container.Resolve<IWeiXinInMobileService>();

                    string message = weiXinInMobileService.CreateWeiXinInMobileFromServer(openId, mobileUserName, numButton, weiXinUserExtension.CompanyCode);

                    if (!String.IsNullOrEmpty(message))
                    {
                        if (message == "200")
                        {
                            Response.Redirect("wxmobilelist.aspx");
                        }
                        else
                        {
                            errorMessage = message;
                        }
                    }
                }
            }
        }
    }
}