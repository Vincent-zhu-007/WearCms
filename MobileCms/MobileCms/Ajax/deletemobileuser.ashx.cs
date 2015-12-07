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
    /// Summary description for deletemobileuser
    /// </summary>
    public class deletemobileuser : IHttpHandler, IRequiresSessionState
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

                if (tempMobileUser == null)
                {
                    jsonObject.Add("message", "用户数据异常");
                    context.Response.Write(jsonObject);
                    return;
                }

                string ownerUri = tempMobileUser.OwnerUri;
                string listType = "WhiteList";

                DeleteMobileContact(ownerUri, listType);
                DeleteMobileApp(ownerUri, listType);

                mobileUserService.Delete(id);

                jsonObject.Add("message", "200");
                context.Response.Write(jsonObject);
                return;
            }
        }

        private void DeleteMobileContact(string ownerUri, string listType)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IMobileContactService mobileContactService = container.Resolve<IMobileContactService>();
            IMobileContactMemberService mobileContactMemberService = container.Resolve<IMobileContactMemberService>();

            MobileContact mobileContact = mobileContactService.GetMobileContactByOwnerUriAndListType(ownerUri, listType);

            if (mobileContact != null)
            {
                string listFileName = mobileContact.ListFileName;

                List<MobileContactMember> mobileContactMembers = mobileContactMemberService.GetMobileContactMembersByOwnerUriAndListFileName(ownerUri, listFileName);

                if (mobileContactMembers != null && mobileContactMembers.Count > 0)
                {
                    foreach (MobileContactMember mobileContactMember in mobileContactMembers)
                    {
                        mobileContactMemberService.Delete(mobileContactMember.Id);
                    }
                }

                mobileContactService.Delete(mobileContact.Id);
            }
        }

        private void DeleteMobileApp(string ownerUri, string listType)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IMobileAppService mobileAppService = container.Resolve<IMobileAppService>();

            MobileApp mobileApp = mobileAppService.GetMobileAppByOwnerUriAndListType(ownerUri, listType);

            if (mobileApp != null)
            {
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

                mobileAppService.Delete(mobileApp.Id);
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