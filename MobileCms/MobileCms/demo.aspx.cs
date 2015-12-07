using System;
using System.IO;
using System.Net;
using System.Text;
using MobileCms.Common;
using Newtonsoft.Json.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using MobileCms.Container;
using MobileCms.Website;

namespace MobileCms
{
    public partial class demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private void SendMqttMessage()
        {
            // create client instance 
            MqttClient client = new MqttClient(IPAddress.Parse("123.56.105.22"));

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            JObject jsonObject = new JObject();
            jsonObject.Add("targetUser", "xxxxx");
            jsonObject.Add("method", "UpdateMobileContact");

            string data = jsonObject.ToString();

            // publish a message on "/home/temperature" topic with QoS 2 
            client.Publish("/home/temperature", Encoding.UTF8.GetBytes(data), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received
            string message = Encoding.UTF8.GetString(e.Message);
        }

        public void CreateMobileUserLocationJson(string companyCode)
        {
            string mobileServerHost = "http://123.57.245.79";
            string mobileServerPort = "8080";
            string mobileServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string mobileServerPart1 = "mobile.mobileuserlocation.create";
            string mobileServerPart2 = ConfigHelp.GetConfigString("GlobalPart");

            string url = mobileServerHost + ":" + mobileServerPort + "/" + mobileServerAppName + "/" + mobileServerPart1 + "/" + mobileServerPart2;

            JObject jsonObjectPara = new JObject();
            jsonObjectPara.Add("userName", "000100000000014");
            jsonObjectPara.Add("longitude", "108.883658");
            jsonObjectPara.Add("latitude", "34.232615");
            jsonObjectPara.Add("address", "");

            string content = jsonObjectPara.ToString();
            string contentType = "text/json";

            HttpWebResponse httpWebResponse = HttpWebRequestHelp.CreatePostHttpResponse(url, contentType, content, null, null, System.Text.Encoding.UTF8, null, companyCode);
            Stream reader = httpWebResponse.GetResponseStream();
            StreamReader sr = new StreamReader(reader, Encoding.UTF8);
            string jsonData = sr.ReadToEnd();
            reader.Close();
        }

        public void SendSms()
        {
            string mobilePhone = "13299131698";
            string smsContent = "【西安诺维】您的注册验证码是123456。如非本人操作，请忽略本短信";
            string optionCategory = "WxRegister";

            string code = SmsHelp.SendSms(mobilePhone, smsContent, optionCategory);
        }

        public void Sha1()
        {
            string token = "f912d37f989411e58ea77845c40712ab";
            string timestamp = "1449028668";
            string nonce = "1092884333";

            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);
            string code = string.Join("", ArrTmp);  

            string result = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(code, "SHA1").ToLower();
        }

        public void CreateBaby()
        {
            string openId = "opi0SuCFB_sy3wVQWkeM_UOGPTdc";
            string mobileUserName = "000000000000000";
            string numButton = "";

            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IWeiXinInMobileService weiXinInMobileService = container.Resolve<IWeiXinInMobileService>();

            string message = weiXinInMobileService.CreateWeiXinInMobileFromServer(openId, mobileUserName, numButton, "DemoCompany");
        }
    }
}