using System;
using MobileCms.Common;

namespace MobileCms
{
    public partial class createmobileappwhitelistitem : BasePageMember
    {
        public string mobileAppId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            mobileAppId = RequestHelp.GetString("id");
        }
    }
}