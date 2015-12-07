using System;
using System.Web.UI;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms
{
    public partial class editmobileappconfig : BasePageMember
    {
        public string strId = "";
        public string description = "";
        public string packageName = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string id = RequestHelp.GetString("id");

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileAppConfigService mobileAppConfigService = container.Resolve<IMobileAppConfigService>();

                MobileAppConfig mobileAppConfig = mobileAppConfigService.GetById(id);
                if (mobileAppConfig != null)
                {
                    strId = id;
                    description = String.IsNullOrEmpty(mobileAppConfig.Description) ? "" : mobileAppConfig.Description;
                    packageName = String.IsNullOrEmpty(mobileAppConfig.PackageName) ? "" : mobileAppConfig.PackageName;
                }
            }
        }
    }
}