using System.Web;
using System.Web.SessionState;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for getmobileuserfile
    /// </summary>
    public class getmobileuserfile : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            string result = "";

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                string id = RequestHelp.GetString("id");

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileUserFileService mobileUserFileService = container.Resolve<IMobileUserFileService>();

                MobileUserFile mobileUserFile = mobileUserFileService.GetById(id);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (mobileUserFile != null)
                {
                    string baseUrl = ConfigHelp.GetConfigString("MobileServerHost");
                    string port = ConfigHelp.GetConfigString("MobileServerPort");

                    string fileUrl = mobileUserFile.FileUrl == null ? "" : baseUrl + ":" + port + mobileUserFile.FileUrl;

                    sb.Append("<audio id=\"audio_" + mobileUserFile.Id + "\" controls=\"controls\" style=\"width:260px;\">");
                    sb.Append("<source src=\"" + fileUrl + "\" />");
                    sb.Append("</audio>");
                }

                result = sb.ToString();

                context.Response.Write(result);
                return;
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