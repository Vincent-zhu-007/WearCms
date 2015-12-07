using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;

namespace MobileCms.WebsiteService
{
    public class WeiXinInMobileService : IWeiXinInMobileService
    {
        public string CreateWeiXinInMobileFromServer(string openId, string mobileUserName, string numButton, string companyCode)
        {
            string resultData = "";

            JObject jObject = new JObject();
            jObject.Add("openId", openId);
            jObject.Add("mobileUserName", mobileUserName);
            jObject.Add("numButton", numButton);

            string mobileServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string mobileServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string mobileServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string mobileServerPart1 = "weixin.weixininmobile.createweixininmobilejson";
            string mobileServerPart2 = ConfigHelp.GetConfigString("GlobalPart");

            string url = mobileServerHost + ":" + mobileServerPort + "/" + mobileServerAppName + "/" + mobileServerPart1 + "/" + mobileServerPart2;
            string content = jObject.ToString();
            string contentType = "text/json";

            HttpWebResponse httpWebResponse = HttpWebRequestHelp.CreatePostHttpResponse(url, contentType, content, null, null, System.Text.Encoding.UTF8, null, companyCode);
            Stream reader = httpWebResponse.GetResponseStream();
            StreamReader sr = new StreamReader(reader, Encoding.UTF8);
            string jsonData = sr.ReadToEnd();
            reader.Close();

            if (!String.IsNullOrEmpty(jsonData))
            {
                JObject resultJsonObject = JObject.Parse(jsonData);

                string result = "";
                bool isContainResult = resultJsonObject.Properties().Any(m => m.Name == "result");
                if (isContainResult)
                {
                    result = resultJsonObject.Property("result").Value.ToString();
                }

                if (result.Equals("1"))
                {
                    bool isContainData = resultJsonObject.Properties().Any(m => m.Name == "data");
                    if (isContainData)
                    {
                        resultData = resultJsonObject.Property("data").Value.ToString();
                    }
                }
                else
                {
                    bool isContainMessage = resultJsonObject.Properties().Any(m => m.Name == "message");
                    if (isContainMessage)
                    {
                        resultData = resultJsonObject.Property("message").Value.ToString();
                    }
                }
            }

            return resultData;
        }

        public List<WeiXinInMobile> GetWeiXinInMobileByWeiXinOwnerUri(string weiXinOwnerUri)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.WeiXinInMobile.Where(m => m.WeiXinOwnerUri == weiXinOwnerUri).ToList();
            }
        }
    }
}
