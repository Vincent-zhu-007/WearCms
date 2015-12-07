using System;
using MobileCms.Data;

namespace MobileCms
{
    public partial class wxregister : System.Web.UI.Page
    {
        public string openId = "";
        public string pageTitle = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["WeiXinAuthorization"] != null && Session["WeiXinAuthorization"].ToString() != "")
            {
                WeiXinAuthorization weiXinAuthorization = Session["WeiXinAuthorization"] as WeiXinAuthorization;

                openId = weiXinAuthorization.OpenId;

                pageTitle = "诺维小天使";
            }
        }
    }
}