using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for getremoveandaddmobilemessagehtmloptions
    /// </summary>
    public class getremoveandaddmobilemessagehtmloptions : IHttpHandler, IRequiresSessionState
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
                IMobileMessageService mobileMessageService = container.Resolve<IMobileMessageService>();

                MobileMessage mobileMessage = mobileMessageService.GetById(id);

                IMobileUserService mobileUserService = container.Resolve<IMobileUserService>();
                Dictionary<string, string> dic = mobileUserService.GetMobileUserDicByCompanyCode(mobileUser.CompanyCode);

                string listboxAddOptions = "";
                if (mobileMessage != null)
                {
                    if (!String.IsNullOrEmpty(mobileMessage.TargetUris))
                    {
                        string[] array = mobileMessage.TargetUris.Split(',');

                        for (int i = 0; i < array.Length; i++)
                        {
                            string targetUris = array[i];

                            string description = "";
                            if (dic.ContainsKey(targetUris))
                            {
                                description = dic[targetUris].ToString();

                                listboxAddOptions += "<option value='" + targetUris + "'>" + description + "</option>";
                            }

                            dic.Remove(targetUris);
                        }
                    }
                }

                string listboxRemoveOptions = "";
                foreach (string key in dic.Keys)
                {
                    listboxRemoveOptions += "<option value='" + key + "'>" + dic[key] + "</option>";
                }

                result = listboxAddOptions + "&" + listboxRemoveOptions;

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