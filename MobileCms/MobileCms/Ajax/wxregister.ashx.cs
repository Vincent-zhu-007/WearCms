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
    /// Summary description for wxregister
    /// </summary>
    public class wxregister : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObjectResult = new JObject();

            string openId = "";
            if (context.Session["WeiXinAuthorization"] != null && context.Session["WeiXinAuthorization"].ToString() != "")
            {
                WeiXinAuthorization weiXinAuthorization = context.Session["WeiXinAuthorization"] as WeiXinAuthorization;

                openId = weiXinAuthorization.OpenId;
            }
            else
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "缺少OpenId");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            string imageCode = RequestHelp.GetString("imageCode");

            if (String.IsNullOrEmpty(imageCode))
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "请输入验证码");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            if (!context.Request.Cookies["CheckCode"].Value.ToLower().Equals(imageCode.ToLower()))
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "验证码有误，请重新获取");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            string password = RequestHelp.GetString("password");
            string rePassword = RequestHelp.GetString("rePassword");

            if (password != rePassword)
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "密码与确认密码不一致");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }
            
            string mobilePhone = RequestHelp.GetString("mobilePhone");
            string displayName = RequestHelp.GetString("displayName");
            string smsCode = RequestHelp.GetString("smsCode");

            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IWeiXinUserService weiXinUserService = container.Resolve<IWeiXinUserService>();

            WeiXinUser weiXinUser = weiXinUserService.GetWeiXinUserByOpenId(openId);

            if (weiXinUser != null)
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "账户已经存在");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }
            
            ISmsService smsService = container.Resolve<ISmsService>();
            Sms sms = smsService.GetSmsByMobileAndOptionCategory(mobilePhone, "WxRegister");

            if (sms == null)
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "未发送校验码至短信");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            DateTime sendTime = Convert.ToDateTime(sms.SendTime);
            if (sendTime.AddMinutes(30) < DateTime.Now)
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "您获取的校验码已经过期，请重新获取");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            if (!sms.SmsCode.Equals(smsCode))
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "手机验证码有误，请重新获取");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            string md5Password = DEncryptHelp.Encrypt(password, 1).ToLower();

            string id = Guid.NewGuid().ToString();
            string publicId = ConfigHelp.GetConfigString("WeiXinPublicId_BDXTS");
            string ownerUri = "wx:" + mobilePhone + "@" + publicId + ".com";
            string userName = mobilePhone + "@" + publicId;

            WeiXinUserExtension model = new WeiXinUserExtension();
            model.Id = id;
            model.OwnerUri = ownerUri;
            model.UserName = userName;
            model.Password = md5Password;
            model.DisplayName = displayName;

            string orgStructure = ConfigHelp.GetConfigString("CompanyCode");
            model.CompanyCode = orgStructure;
            model.OrgStructure = orgStructure;

            model.Status = "Y";
            model.CreateTime = DateTime.Now;
            model.Creator = ownerUri;

            model.MobilePhone = mobilePhone;
            model.IsOpenExtension = "N";

            model.OpenId = openId;

            weiXinUserService.Create(model);

            context.Session.Add("WeiXinUserId", model.Id);
            context.Session.Add("WeiXinUserName", model.UserName);

            CookieHelp.SetCookie("WeiXinUserName", userName);
            CookieHelp.SetDESEncryptedCookie("WeiXinUserPwd", password);

            jsonObjectResult.Add("result", "1");
            jsonObjectResult.Add("data", "200");
            context.Response.Write(jsonObjectResult.ToString());
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