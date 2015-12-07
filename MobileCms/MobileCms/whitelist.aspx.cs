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
    public partial class whitelist : BasePageMember
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WeiXinUserExtension weiXinUserExtension = GetWeiXinUserExtension();

                string userName = RequestHelp.GetString("userName");

                GetList(weiXinUserExtension, userName);
            }
        }

        private void GetList(WeiXinUserExtension weiXinUserExtension, string userName)
        {
            int currentPageIndex = RequestHelp.GetQueryInt("page1", 1);
            int start = RequestHelp.GetQueryInt("page1", 0);
            int limit = 10;
            int total = 0;

            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IMobileContactService mobileContactService = container.Resolve<IMobileContactService>();

            DataTable dt = mobileContactService.GetListByPage(weiXinUserExtension.CompanyCode, userName, "WhiteList", start, limit, out total);
            rptList.DataSource = dt;
            rptList.DataBind();

            AspNetPageBind(dt, total, limit, currentPageIndex);
        }

        protected void butSearch_Click(object sender, EventArgs e)
        {
            WeiXinUserExtension weiXinUserExtension = GetWeiXinUserExtension();

            string userName = this.userName.Text;

            GetList(weiXinUserExtension, userName);
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