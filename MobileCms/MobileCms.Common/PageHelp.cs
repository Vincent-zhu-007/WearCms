using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace MobileCms.Common
{
    public class PageHelp
    {
        public PageHelp()
        {
            PageSize = 10;
            PageNumHtml = new System.Text.StringBuilder();
            UrlPageIndexName = "page";
        }

        private int _pageCount;
        private int _currentPageIndex;
        private int _recordCount;
        private int _pageSize;
        public string _urlRewritePattern;
        public System.Text.StringBuilder _pageNumHtml;
        public DataSet _dataSource;
        public string _customInfoHTML;
        public string _urlPageIndexName;

        public PagedDataSource Page()
        {
            PagedDataSource pagedDataSource = new PagedDataSource();

            pagedDataSource.DataSource = DataSource.Tables[0].DefaultView;

            pagedDataSource.AllowPaging = true;

            RecordCount = DataSource.Tables[0].Rows.Count;

            pagedDataSource.PageSize = PageSize;

            CurrentPageIndex = RequestHelp.GetInt(UrlPageIndexName, 1);

            pagedDataSource.CurrentPageIndex = CurrentPageIndex - 1;

            int nextPageIndex = 0;
            int prevPageIndex = 0;
            int startcount = 0;
            int endcount = 0;

            if (CurrentPageIndex < 1)
            {
                CurrentPageIndex = 1;
            }
            //计算总页数
            if (PageSize != 0)
            {
                PageCount = (RecordCount / PageSize);

                PageCount = ((RecordCount % PageSize) != 0 ? PageCount + 1 : PageCount);

                PageCount = (PageCount == 0 ? 1 : PageCount);
            }

            nextPageIndex = CurrentPageIndex + 1;

            prevPageIndex = CurrentPageIndex - 1;

            startcount = (CurrentPageIndex + 5) > PageCount ? PageCount - 9 : CurrentPageIndex - 4;//中间页起始序号

            //中间页终止序号

            endcount = CurrentPageIndex < 5 ? 10 : CurrentPageIndex + 5;

            if (startcount < 1)
            {
                startcount = 1;
            }
            //为了避免输出的时候产生负数，设置如果小于1就从序号1开始

            if (PageCount < endcount)
            {
                endcount = PageCount;
            }

            //页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            if (string.IsNullOrEmpty(CustomInfoHTML))
            {
                CustomInfoHTML = "当前页:<font color='red'><b>" + CurrentPageIndex + "</b></font>&nbsp;&nbsp;总页数:<b>" + PageCount + "</b>&nbsp;&nbsp;总记录数:<b>" + RecordCount + "</b>   ";
            }

            PageNumHtml.Append(CustomInfoHTML);

            if (string.IsNullOrEmpty(UrlRewritePattern))
            {
                UrlRewritePattern = HttpContext.Current.Request.CurrentExecutionFilePath.Substring(1, HttpContext.Current.Request.CurrentExecutionFilePath.LastIndexOf(".") - 1) + "-{0}.aspx";
            }

            PageNumHtml.Append(CurrentPageIndex > 1 ? "<a href=\"" + string.Format(UrlRewritePattern, "1") + "\">首页</a>  <a href=\"" + string.Format(UrlRewritePattern, prevPageIndex.ToString()) + "\">上一页</a>" : "首页 上一页");

            //中间页处理，这个增加时间复杂度，减小空间复杂度

            for (int i = startcount; i <= endcount; i++)
            {
                PageNumHtml.Append(CurrentPageIndex == i ? "  <font color=\"#ff0000\">" + i + "</font>" : "  <a href=\"" + string.Format(UrlRewritePattern, i.ToString()) + "\">" + i + "</a>");
            }

            PageNumHtml.Append(CurrentPageIndex != PageCount ? "  <a href=\"" + string.Format(UrlRewritePattern, nextPageIndex.ToString()) + "\">下一页</a>  <a href=\"" + string.Format(UrlRewritePattern, PageCount) + "\">末页</a>" : " 下一页 末页");

            return pagedDataSource;
        }



        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set { _currentPageIndex = value; }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        /// <summary>
        /// 路径
        /// </summary>
        public string UrlRewritePattern
        {
            get { return _urlRewritePattern; }
            set { _urlRewritePattern = value; }
        }

        public System.Text.StringBuilder PageNumHtml
        {
            get { return _pageNumHtml; }
            set { _pageNumHtml = value; }
        }
        /// <summary>
        /// 数据源
        /// </summary>
        public DataSet DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        public string CustomInfoHTML
        {
            get { return _customInfoHTML; }
            set { _customInfoHTML = value; }
        }

        public string UrlPageIndexName
        {
            get { return _urlPageIndexName; }
            set { _urlPageIndexName = value; }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="dataTable">数据源</param>
        /// <param name="pageSize">每页多少条</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="pager"></param>

        public PagedDataSource PageDataSource(DataTable dataTable, int pageSize, int currentPageIndex, Wuqi.Webdiyer.AspNetPager pager)
        {
            DataView dataView = dataTable.DefaultView;
            string sortExpression = HttpContext.Current.Request["sortExpression"];
            string sortDirection = HttpContext.Current.Request["sortDirection"];
            if ((!string.IsNullOrEmpty(sortExpression)) && (!string.IsNullOrEmpty(sortDirection)))
            {
                dataView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
            }
            PagedDataSource myPage = new PagedDataSource();
            myPage.DataSource = dataView;
            pager.RecordCount = dataTable.Rows.Count;
            myPage.AllowPaging = true;
            myPage.PageSize = pageSize;
            pager.PageSize = myPage.PageSize;
            pager.UrlPaging = true;
            pager.FirstPageText = "首页";
            pager.PrevPageText = "上一页";
            pager.NextPageText = "下一页";
            pager.LastPageText = "尾页";

            myPage.CurrentPageIndex = currentPageIndex - 1;
            return myPage;
        }

        public PagedDataSource PageDataSource2(DataTable dataTable, int pageSize, int currentPageIndex, Wuqi.Webdiyer.AspNetPager pager)
        {
            DataView dataView = dataTable.DefaultView;
            string sortExpression = HttpContext.Current.Request["sortExpression"];
            string sortDirection = HttpContext.Current.Request["sortDirection"];
            if ((!string.IsNullOrEmpty(sortExpression)) && (!string.IsNullOrEmpty(sortDirection)))
            {
                dataView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
            }
            PagedDataSource myPage = new PagedDataSource();
            myPage.DataSource = dataView;
            pager.RecordCount = dataTable.Rows.Count;
            myPage.AllowPaging = true;
            myPage.PageSize = pageSize;
            pager.PageSize = myPage.PageSize;
            pager.UrlPaging = true;

            pager.FirstPageText = "<<";
            pager.PrevPageText = "<";
            pager.NextPageText = ">";
            pager.LastPageText = ">>";
            pager.CssClass = "paginator";
            pager.CurrentPageButtonClass = "cursor";
            pager.ShowPageIndexBox = Wuqi.Webdiyer.ShowPageIndexBox.Never;

            myPage.CurrentPageIndex = currentPageIndex - 1;
            return myPage;
        }

        public PagedDataSource PageDataSource3(DataTable dataTable, int pageSize, int currentPageIndex, Wuqi.Webdiyer.AspNetPager pager)
        {
            DataView dataView = dataTable.DefaultView;
            string sortExpression = HttpContext.Current.Request["sortExpression"];
            string sortDirection = HttpContext.Current.Request["sortDirection"];
            if ((!string.IsNullOrEmpty(sortExpression)) && (!string.IsNullOrEmpty(sortDirection)))
            {
                dataView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
            }
            PagedDataSource myPage = new PagedDataSource();
            myPage.DataSource = dataView;
            pager.RecordCount = dataTable.Rows.Count;
            myPage.AllowPaging = true;
            myPage.PageSize = pageSize;
            pager.PageSize = myPage.PageSize;
            pager.UrlPaging = true;
            pager.FirstPageText = "首页";
            pager.PrevPageText = "上一页";
            pager.NextPageText = "下一页";
            pager.LastPageText = "尾页";
            pager.CssClass = "paginator";
            pager.CurrentPageButtonClass = "cursor";
            pager.ShowPageIndexBox = Wuqi.Webdiyer.ShowPageIndexBox.Never;

            myPage.CurrentPageIndex = currentPageIndex - 1;
            return myPage;
        }

        public PagedDataSource PageDataSource4(IList datasource, int pageSize, int currentPageIndex, Wuqi.Webdiyer.AspNetPager pager)
        {
            PagedDataSource myPage = new PagedDataSource();
            myPage.DataSource = datasource;
            pager.RecordCount = datasource.Count;
            myPage.AllowPaging = true;
            myPage.PageSize = pageSize;
            pager.PageSize = myPage.PageSize;
            pager.UrlPaging = true;
            pager.FirstPageText = "首页";
            pager.PrevPageText = "上一页";
            pager.NextPageText = "下一页";
            pager.LastPageText = "尾页";
            pager.CssClass = "paginator";
            pager.CurrentPageButtonClass = "cursor";
            pager.ShowPageIndexBox = Wuqi.Webdiyer.ShowPageIndexBox.Never;
            myPage.CurrentPageIndex = currentPageIndex - 1;
            return myPage;
        }

        public PagedDataSource PageDataSource10(DataTable dataTable, int pageSize, int recordCount, int currentPageIndex, Wuqi.Webdiyer.AspNetPager pager)
        {
            DataView dataView = dataTable.DefaultView;
            string sortExpression = HttpContext.Current.Request["sortExpression"];
            string sortDirection = HttpContext.Current.Request["sortDirection"];
            if ((!string.IsNullOrEmpty(sortExpression)) && (!string.IsNullOrEmpty(sortDirection)))
            {
                dataView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
            }
            PagedDataSource myPage = new PagedDataSource();
            myPage.DataSource = dataView;
            pager.RecordCount = recordCount;
            myPage.AllowPaging = true;
            myPage.PageSize = pageSize;
            pager.PageSize = pageSize;
            pager.UrlPaging = true;

            pager.FirstPageText = "<<";
            pager.PrevPageText = "<";
            pager.NextPageText = ">";
            pager.LastPageText = ">>";
            pager.CssClass = "paginator";
            pager.CurrentPageButtonClass = "cursor";
            pager.ShowPageIndexBox = Wuqi.Webdiyer.ShowPageIndexBox.Never;

            myPage.CurrentPageIndex = currentPageIndex - 1;
            return myPage;
        }
    }
}
