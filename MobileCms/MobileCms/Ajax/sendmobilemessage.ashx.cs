﻿using System;
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
    /// Summary description for sendmobilemessage
    /// </summary>
    public class sendmobilemessage : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            String message = "";

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                string companyCode = mobileUser.CompanyCode;

                string id = RequestHelp.GetString("id");
                
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileMessageService mobileMessageService = container.Resolve<IMobileMessageService>();

                string result = mobileMessageService.SendMobileMessageFromServer(companyCode, id);

                jsonObject.Add("message", result);
                context.Response.Write(jsonObject);
                return;
            }

            jsonObject.Add("message", message);
            context.Response.Write(jsonObject);
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