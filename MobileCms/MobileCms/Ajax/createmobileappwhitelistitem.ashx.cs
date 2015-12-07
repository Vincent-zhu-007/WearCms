using System;
using System.Collections.Generic;
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
    /// Summary description for createmobileappwhitelistitem
    /// </summary>
    public class createmobileappwhitelistitem : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JObject jsonObject = new JObject();

            String message = "";

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                string mobileAppId = RequestHelp.GetString("mobileAppId");

                string companyCode = mobileUser.CompanyCode;

                string mobileAppItemAppCodeNames = RequestHelp.GetString("mobileAppItemAppCodeNames");

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileAppConfigService mobileAppConfigService = container.Resolve<IMobileAppConfigService>();

                Dictionary<string, string> mobileAppConfigMap = mobileAppConfigService.GetMobileAppConfigCacheFromServer(companyCode);

                IMobileAppService mobileAppService = container.Resolve<IMobileAppService>();

                MobileApp mobileApp = mobileAppService.GetById(mobileAppId);

                if (mobileApp != null)
                {
                    string ownerUri = mobileApp.OwnerUri;
                    string listFileName = mobileApp.ListFileName;

                    IMobileAppItemService mobileAppItemService = container.Resolve<IMobileAppItemService>();
                    List<MobileAppItem> mobileAppItems = mobileAppItemService.GetMobileAppItemByOwnerUriAndListFileName(ownerUri, listFileName);

                    if (mobileAppItems != null && mobileAppItems.Count > 0)
                    {
                        foreach (MobileAppItem mobileAppItem in mobileAppItems)
                        {
                            mobileAppItemService.Delete(mobileAppItem.Id);
                        }
                    }

                    string[] array = mobileAppItemAppCodeNames.Split(',');

                    for (int i = 0; i < array.Length; i++)
                    {
                        string mobileAppItemAppCodeName = array[i].ToString();

                        MobileAppItem mobileAppItem = new MobileAppItem();
                        mobileAppItem.Id = Guid.NewGuid().ToString();
                        mobileAppItem.OwnerUri = ownerUri;

                        if (!mobileAppConfigMap.ContainsKey(mobileAppItemAppCodeName))
                        {
                            continue;
                        }

                        mobileAppItem.AppCodeName = mobileAppItemAppCodeName;
                        mobileAppItem.ListFileName = listFileName;

                        mobileAppItemService.Create(mobileAppItem);
                    }

                    mobileApp.Etag = Guid.NewGuid().ToString();
                    mobileApp.UpdateTime = DateTime.Now;
                    mobileApp.Updator = mobileUser.OwnerUri;
                    mobileAppService.Update(mobileApp);

                    IRpcService rpcService = container.Resolve<IRpcService>();
                    rpcService.runCmdUpdateApp(ownerUri, companyCode);

                    jsonObject.Add("message", "200");
                    context.Response.Write(jsonObject);
                    return;
                }
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