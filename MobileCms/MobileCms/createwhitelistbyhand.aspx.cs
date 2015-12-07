using System;
using MobileCms.Common;

namespace MobileCms
{
    public partial class createwhitelistbyhand : BasePageMember
    {
        public string mobileContactId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            mobileContactId = RequestHelp.GetString("id");
        }
    }
}