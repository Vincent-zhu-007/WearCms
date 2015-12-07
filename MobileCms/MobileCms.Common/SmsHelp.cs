using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MobileCms.Common
{
    public class SmsHelp
    {
        public static string SendSms(string mobilePhone, string smsContent, string optionCategory)
        {
            string code = "";

            string apikey = ConfigHelp.GetConfigString("SmsApiKey");
            
            //调用短信接口 发送短信
            string url = "http://yunpian.com/v1/sms/send.json";
            
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("apikey", apikey);
            parameters.Add("mobile", mobilePhone);
            parameters.Add("text", smsContent);

            HttpWebResponse httpWebResponse = HttpWebRequestHelp.CreateCommonPostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);
            Stream reader = httpWebResponse.GetResponseStream();
            StreamReader sr = new StreamReader(reader, Encoding.UTF8);
            string result = sr.ReadLine();
            reader.Close();

            if (!String.IsNullOrEmpty(result))
            {
                JObject resultJsonObject = JObject.Parse(result);

                bool isContainCode = resultJsonObject.Properties().Any(m => m.Name == "code");
                if (isContainCode)
                {
                    code = resultJsonObject.Property("code").Value.ToString();
                }
            }

            return code;
        }
    }
}
