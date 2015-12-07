using System;
using System.Web.UI;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms
{
    public partial class editwhitelistmember : BasePageMember
    {
        public string mobileContactMemberId = "";
        public string displayName = "";
        public string mobilePhone = "";
        public string shortNum = "";
        public string numButton = "";
        public string mobileContactId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string id = RequestHelp.GetString("id");

                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileContactMemberService mobileContactMemberService = container.Resolve<IMobileContactMemberService>();

                MobileContactMember mobileContactMember = mobileContactMemberService.GetById(id);
                if (mobileContactMember != null)
                {
                    mobileContactMemberId = mobileContactMember.Id;
                    displayName = mobileContactMember.DisplayName;
                    mobilePhone = mobileContactMember.MobilePhone;
                    shortNum = mobileContactMember.ShortNum == null ? "" : mobileContactMember.ShortNum;
                    numButton = mobileContactMember.NumButton;

                    string ownerUri = mobileContactMember.OwnerUri;
                    string listFileName = mobileContactMember.ListFileName;
                    IMobileContactService mobileContactService = container.Resolve<IMobileContactService>();
                    MobileContact mobileContact = mobileContactService.GetMobileContactByOwnerUriAndListFileName(ownerUri, listFileName);
                    mobileContactId = mobileContact.Id;
                }
            }
        }
    }
}