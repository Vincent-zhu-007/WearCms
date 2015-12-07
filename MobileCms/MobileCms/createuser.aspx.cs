using System;
using MobileCms.Common;
using MobileCms.Data;

namespace MobileCms
{
    public partial class createuser : BasePageMember
    {
        public string genderDdlOptionHtml = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

                genderDdlOptionHtml = HtmlHelp.InitDdlOptionHtml("GENDER", mobileUser.CompanyCode);
            }
        }
    }
}