using System;
using System.Web.UI;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms
{
    public partial class edituser : BasePageMember
    {
        public string genderDdlOptionHtml = "";

        public string strId = "";
        public string displayName = "";
        public string password = "";
        public string gender = "";
        public string birthday = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

                string id = RequestHelp.GetString("id");

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileUserExtensionService mobileUserExtensionService = container.Resolve<IMobileUserExtensionService>();

                MobileUserExtension mobileUserExtension = mobileUserExtensionService.GetMobileUserExtensionById(id);
                if (mobileUserExtension != null)
                {
                    strId = id;
                    displayName = String.IsNullOrEmpty(mobileUserExtension.DisplayName) ? "" : mobileUserExtension.DisplayName;
                    password = String.IsNullOrEmpty(mobileUserExtension.Password) ? "" : mobileUserExtension.Password;
                    gender = String.IsNullOrEmpty(mobileUserExtension.Gender) ? "" : mobileUserExtension.Gender;
                    birthday = String.IsNullOrEmpty(mobileUserExtension.Birthday) ? "" : mobileUserExtension.Birthday;

                    genderDdlOptionHtml = HtmlHelp.InitDdlOptionSelectHtml("GENDER", gender, mobileUser.CompanyCode);
                }
            }
        }
    }
}