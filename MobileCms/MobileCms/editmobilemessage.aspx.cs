using System;
using System.Web.UI;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms
{
    public partial class editmobilemessage : BasePageMember
    {
        public string strId = "";
        public string content = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string id = RequestHelp.GetString("id");

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileMessageService mobileMessageService = container.Resolve<IMobileMessageService>();

                MobileMessage mobileMessage = mobileMessageService.GetById(id);
                if (mobileMessage != null)
                {
                    strId = id;
                    content = String.IsNullOrEmpty(mobileMessage.Content) ? "" : mobileMessage.Content;
                }
            }
        }
    }
}