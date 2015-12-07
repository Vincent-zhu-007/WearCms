using System;
using MobileCms.Common;
using MobileCms.Data;

namespace MobileCms
{
    public partial class wxlogin : System.Web.UI.Page
    {
        public string openId = "";
        public string pageTitle = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            pageTitle = "诺维小天使";

            if (Session["WeiXinAuthorization"] != null && Session["WeiXinAuthorization"].ToString() != "")
            {
                WeiXinAuthorization weiXinAuthorization = Session["WeiXinAuthorization"] as WeiXinAuthorization;

                openId = weiXinAuthorization.OpenId;
            }
            else
            {
                PageAuthorization();
            }
        }

        private void PageAuthorization()
        {
            string appid = ConfigHelp.GetConfigString("WeiXinAppId");
            string redirect_uri = Server.UrlEncode("http://www.snnavi.com/wxcallback.aspx");
            string state = "wxlogin";

            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state=" + state + "#wechat_redirect";

            Response.Redirect(url);
            return;
        }
    }
}