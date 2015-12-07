using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MobileCms.Common;
using MobileCms.Data;
using Newtonsoft.Json.Linq;

namespace MobileCms
{
    public partial class wxcallback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string appid = ConfigHelp.GetConfigString("WeiXinAppId");
            string secret = ConfigHelp.GetConfigString("WeiXinAppSecret");
            
            string code = RequestHelp.GetString("code");
            string state = RequestHelp.GetString("state");

            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code";

            HttpWebResponse httpWebResponse = HttpWebRequestHelp.CreateGetHttpResponse(url, null, null, null);
            Stream reader = httpWebResponse.GetResponseStream();
            StreamReader sr = new StreamReader(reader, Encoding.UTF8);
            string jsonData = sr.ReadToEnd();
            reader.Close();

            if (!String.IsNullOrEmpty(jsonData))
            {
                JObject resultJsonObject = JObject.Parse(jsonData);

                bool isContainErrCode = resultJsonObject.Properties().Any(m => m.Name == "errcode");
                if (isContainErrCode)
                {

                }
                else
                {
                    WeiXinAuthorization weiXinAuthorization = new WeiXinAuthorization();

                    bool isContainAccessToken = resultJsonObject.Properties().Any(m => m.Name == "access_token");
                    if (isContainAccessToken)
                    {
                        weiXinAuthorization.AccessToken = resultJsonObject.Property("access_token").Value.ToString();
                    }

                    bool isContainExpiresIn = resultJsonObject.Properties().Any(m => m.Name == "expires_in");
                    if (isContainExpiresIn)
                    {
                        weiXinAuthorization.ExpiresIn = resultJsonObject.Property("expires_in").Value.ToString();
                    }

                    bool isContainRefreshToken = resultJsonObject.Properties().Any(m => m.Name == "refresh_token");
                    if (isContainRefreshToken)
                    {
                        weiXinAuthorization.RefreshToken = resultJsonObject.Property("refresh_token").Value.ToString();
                    }

                    bool isContainOpenId = resultJsonObject.Properties().Any(m => m.Name == "openid");
                    if (isContainOpenId)
                    {
                        weiXinAuthorization.OpenId = resultJsonObject.Property("openid").Value.ToString();
                    }

                    bool isContainScope = resultJsonObject.Properties().Any(m => m.Name == "scope");
                    if (isContainScope)
                    {
                        weiXinAuthorization.Scope = resultJsonObject.Property("scope").Value.ToString();
                    }

                    bool isContainUnionId = resultJsonObject.Properties().Any(m => m.Name == "unionid");
                    if (isContainUnionId)
                    {
                        weiXinAuthorization.UnionId = resultJsonObject.Property("unionid").Value.ToString();
                    }

                    Session["WeiXinAuthorization"] = weiXinAuthorization;

                    Response.Redirect(state + ".aspx");
                    return;
                }
            }
        }
    }
}