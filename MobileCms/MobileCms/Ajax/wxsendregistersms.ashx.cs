using System;
using System.Text.RegularExpressions;
using System.Web;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for wxsendregistersms
    /// </summary>
    public class wxsendregistersms : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObjectResult = new JObject();

            string openId = RequestHelp.GetString("openId");
            string mobilePhone = RequestHelp.GetString("mobilePhone");

            if (String.IsNullOrEmpty(mobilePhone))
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "请输入手机号码");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            if (String.IsNullOrEmpty(openId))
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "缺少OpenId");
                context.Response.Write(jsonObjectResult.ToString());
                return;
            }

            string optionCategory = "WxRegister";

            string regex1 = @"^13[0-9]{9}";
            string regex2 = @"^15[012356789][0-9]{8}";
            string regex3 = @"^18[0123456789][0-9][0-9]";
            string regex4 = @"^147[0-9]{8}$";
            string regex5 = @"^170[0-9]{8}$";
            string regex6 = @"^177[0-9]{8}$";

            bool mobilePhoneIsPass1 = Regex.IsMatch(mobilePhone, regex1);
            bool mobilePhoneIsPass2 = Regex.IsMatch(mobilePhone, regex2);
            bool mobilePhoneIsPass3 = Regex.IsMatch(mobilePhone, regex3);
            bool mobilePhoneIsPass4 = Regex.IsMatch(mobilePhone, regex4);
            bool mobilePhoneIsPass5 = Regex.IsMatch(mobilePhone, regex5);
            bool mobilePhoneIsPass6 = Regex.IsMatch(mobilePhone, regex6);

            bool mobilePhoneIsPass = false;
            if (mobilePhoneIsPass1 || mobilePhoneIsPass2 || mobilePhoneIsPass3 || mobilePhoneIsPass4 || mobilePhoneIsPass5 || mobilePhoneIsPass6)
            {
                mobilePhoneIsPass = true;
            }

            if (!mobilePhoneIsPass)
            {
                jsonObjectResult.Add("result", "0");
                jsonObjectResult.Add("message", "手机号码不正确");
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
            Sms sms = smsService.GetSmsByMobileAndOptionCategory(mobilePhone, optionCategory);

            string smsCode = DEncryptHelp.GetRandWordNum(6);

            if (sms != null)
            {
                DateTime sendTime = Convert.ToDateTime(sms.SendTime);
                if (sendTime.AddMinutes(30) > DateTime.Now)
                {
                    jsonObjectResult.Add("result", "0");
                    jsonObjectResult.Add("message", "您已经获取校验码，请查看短信");
                    context.Response.Write(jsonObjectResult.ToString());
                    return;
                }

                string smsContent = "【西安诺维】您的注册验证码是" + smsCode + "。如非本人操作，请忽略本短信";

                //调用短信接口 发送短信
                string sendSmsResult = SmsHelp.SendSms(mobilePhone, smsContent, optionCategory);

                if (sendSmsResult == "0")
                {
                    sms.SmsCode = smsCode;
                    sms.SendTime = DateTime.Now;

                    smsService.Update(sms);

                    jsonObjectResult.Add("result", "1");
                    jsonObjectResult.Add("data", "200");
                    context.Response.Write(jsonObjectResult.ToString());
                    return;
                }
                else
                {
                    jsonObjectResult.Add("result", "0");
                    jsonObjectResult.Add("message", "短信发送失败，请稍后再试，错误码[" + sendSmsResult + "]");
                    context.Response.Write(jsonObjectResult.ToString());
                    return;
                }
            }
            else
            {
                string smsContent = "【西安诺维】您的注册验证码是" + smsCode + "。如非本人操作，请忽略本短信";

                //调用短信接口 发送短信
                string sendSmsResult = SmsHelp.SendSms(mobilePhone, smsContent, optionCategory);

                if (sendSmsResult == "0")
                {
                    Sms model = new Sms();
                    model.Id = Guid.NewGuid().ToString();
                    model.Mobile = mobilePhone;
                    model.OptionCategory = optionCategory;
                    model.SmsCode = smsCode;
                    model.CreateTime = DateTime.Now;
                    model.SendTime = DateTime.Now;

                    smsService.Create(model);

                    jsonObjectResult.Add("result", "1");
                    jsonObjectResult.Add("data", "200");
                    context.Response.Write(jsonObjectResult.ToString());
                }
                else
                {
                    jsonObjectResult.Add("result", "0");
                    jsonObjectResult.Add("message", "短信发送失败，请稍后再试，错误码[" + sendSmsResult + "]");
                    context.Response.Write(jsonObjectResult.ToString());
                    return;
                }
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