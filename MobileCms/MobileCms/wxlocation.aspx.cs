using System;
using MobileCms.Common;
using MobileCms.Data;

namespace MobileCms
{
    public partial class wxlocation : BasePageWeiXinMember
    {
        public string openId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["WeiXinAuthorization"] != null && Session["WeiXinAuthorization"].ToString() != "")
            {
                WeiXinAuthorization weiXinAuthorization = Session["WeiXinAuthorization"] as WeiXinAuthorization;

                openId = weiXinAuthorization.OpenId;
            }
        }
    }
}