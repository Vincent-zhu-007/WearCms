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
    /// Summary description for deletemobileappconfig
    /// </summary>
    public class deletemobileappconfig : IHttpHandler, IRequiresSessionState
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
                IMobileAppConfigService mobileAppConfigService = container.Resolve<IMobileAppConfigService>();

                MobileAppConfig tempMobileAppConfig = mobileAppConfigService.GetById(id);

                if (tempMobileAppConfig == null)
                {
                    jsonObject.Add("message", "用户数据异常");
                    context.Response.Write(jsonObject);
                    return;
                }

                DeleteMobileAppItem(tempMobileAppConfig.CodeName, mobileUser);

                mobileAppConfigService.Delete(id);

                jsonObject.Add("message", "200");
                context.Response.Write(jsonObject);
                return;
            }
        }

        private void DeleteMobileAppItem(string appCodeName, MobileUser mobileUser)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IMobileAppItemService mobileAppItemService = container.Resolve<IMobileAppItemService>();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            List<MobileAppItem> mobileAppItems = mobileAppItemService.GetMobileAppItemsByAppCodeName(appCodeName);

            if (mobileAppItems != null && mobileAppItems.Count > 0)
            {
                foreach (MobileAppItem mobileAppItem in mobileAppItems)
                {
                    if (dic.ContainsKey(mobileAppItem.OwnerUri))
                    {
                        continue;
                    }
                    else
                    {
                        dic.Add(mobileAppItem.OwnerUri, mobileAppItem.ListFileName);
                    }
                    mobileAppItemService.Delete(mobileAppItem.Id);
                }

                if (dic.Count > 0)
                {
                    IMobileAppService mobileAppService = container.Resolve<IMobileAppService>();

                    foreach (string key in dic.Keys)
                    {
                        MobileApp mobileApp = mobileAppService.GetMobileAppByOwnerUriAndListFileName(key, dic[key]);

                        if (mobileApp != null)
                        {
                            mobileApp.Etag = Guid.NewGuid().ToString();
                            mobileApp.UpdateTime = DateTime.Now;
                            mobileApp.Updator = mobileUser.OwnerUri;
                            mobileAppService.Update(mobileApp);
                        }
                    }
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