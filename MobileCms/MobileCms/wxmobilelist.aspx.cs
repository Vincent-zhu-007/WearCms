using System;
using MobileCms.Common;
using MobileCms.Data;
using MobileCms.Container;
using MobileCms.Website;
using System.Collections.Generic;

namespace MobileCms
{
    public partial class wxmobilelist : BasePageWeiXinMember
    {
        public string mobileListHtml = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            InitMobileListHtml();
        }

        private void InitMobileListHtml()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            WeiXinUserExtension weiXinUserExtension = GetWeiXinUserExtension();

            if (weiXinUserExtension != null)
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IWeiXinInMobileService weiXinInMobileService = container.Resolve<IWeiXinInMobileService>();

                List<WeiXinInMobile> weiXinInMobiles = weiXinInMobileService.GetWeiXinInMobileByWeiXinOwnerUri(weiXinUserExtension.OwnerUri);

                if (weiXinInMobiles != null && weiXinInMobiles.Count > 0)
                {
                    sb.Append("<ul>");

                    foreach (WeiXinInMobile weiXinInMobile in weiXinInMobiles)
                    {
                        sb.Append("<li>");
                        sb.Append(weiXinInMobile.MobileOwnerUri);
                        sb.Append("</li>");
                    }

                    sb.Append("</ul>");
                }
            }

            mobileListHtml = sb.ToString();
        }
    }
}