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
    /// Summary description for listeningmobile
    /// </summary>
    public class listeningmobile : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            string id = RequestHelp.GetString("id");

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileUserService mobileUserService = container.Resolve<IMobileUserService>();

                MobileUser tempMobileUser = mobileUserService.GetMobileUserById(id);

                if (tempMobileUser != null)
                {
                    string mqttTopic = "host/" + tempMobileUser.UserName + "";
                    JObject jsonObjectMqttData = new JObject();
                    jsonObjectMqttData.Add("m", "ListeningMobile");

                    string mqttData = jsonObjectMqttData.ToString();

                    MqttHelp.SendMqttMessage(mqttTopic, mqttData);

                    jsonObject.Add("message", "200");
                    context.Response.Write(jsonObject);
                    return;
                }
                else
                {
                    jsonObject.Add("message", "没有相关用户");
                    context.Response.Write(jsonObject);
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