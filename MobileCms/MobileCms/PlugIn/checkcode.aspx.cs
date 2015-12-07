using System;
using System.Web;
using MobileCms.Common;

namespace MobileCms.PlugIn
{
    public partial class checkcode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = -1;
            if (!this.Page.IsPostBack)
            {
                this.CreateCheckCode();
            }
        }

        private void CreateCheckCode()
        {
            ImageHelp img = new ImageHelp();
            string checkCode = DEncryptHelp.GetRandWord(5);
            Response.Cookies.Add(new HttpCookie("CheckCode", checkCode));
            img.CreateCheckImage(checkCode);
            img = null;
        }
    }
}