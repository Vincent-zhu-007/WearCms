using System;

namespace MobileCms.Common
{
    public class BasePage : System.Web.UI.Page
    {
        public BasePage()
        {
            this.Load += new EventHandler(PageBase_Load);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(PageBase_Load);
            this.Error += new System.EventHandler(PageBase_Error);
        }

        private void PageBase_Load(object sender, EventArgs e)
        {

        }

        private void PageBase_Error(object sender, System.EventArgs e)
        {

        }
    }
}
