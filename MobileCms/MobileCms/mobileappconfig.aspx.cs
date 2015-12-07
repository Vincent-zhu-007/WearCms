using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms
{
    public partial class mobileappconfig : BasePageMember
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MobileUser mobileUser = GetMobileUser();

                string description = RequestHelp.GetString("description");

                GetList(mobileUser, description);
            }
        }

        private void GetList(MobileUser mobileUser, string description)
        {
            int currentPageIndex = RequestHelp.GetQueryInt("page1", 1);
            int start = RequestHelp.GetQueryInt("page1", 0);
            int limit = 10;
            int total = 0;

            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IMobileAppConfigService mobileAppConfigService = container.Resolve<IMobileAppConfigService>();
            DataTable dt = mobileAppConfigService.GetListByPage(mobileUser.CompanyCode, description, start, limit, out total);
            rptList.DataSource = dt;
            rptList.DataBind();

            AspNetPageBind(dt, total, limit, currentPageIndex);
        }

        protected void butSearch_Click(object sender, EventArgs e)
        {
            MobileUser mobileUser = GetMobileUser();

            string description = this.description.Text;

            GetList(mobileUser, description);
        }

        protected void AspNetPageBind(DataTable dt, int total, int limit, int currentPageIndex)
        {
            DataView dataView = dt.DefaultView;
            PagedDataSource myPage = new PagedDataSource();
            myPage.DataSource = dataView;
            AspNetPager1.RecordCount = total;
            myPage.AllowPaging = true;
            myPage.PageSize = limit;
            AspNetPager1.PageSize = limit;
            AspNetPager1.UrlPaging = true;

            AspNetPager1.FirstPageText = "<<";
            AspNetPager1.PrevPageText = "<";
            AspNetPager1.NextPageText = ">";
            AspNetPager1.LastPageText = ">>";
            AspNetPager1.CssClass = "paginator";
            AspNetPager1.CurrentPageButtonClass = "cursor";
            AspNetPager1.ShowPageIndexBox = Wuqi.Webdiyer.ShowPageIndexBox.Never;

            myPage.CurrentPageIndex = currentPageIndex - 1;
        }
    }
}