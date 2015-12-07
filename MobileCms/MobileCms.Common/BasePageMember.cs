using System;
using System.Web;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.Common
{
    public class BasePageMember : BasePage
    {
        public BasePageMember()
        {
            this.Load += new EventHandler(PageBase_Load);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(PageBase_Load);

        }

        private void PageBase_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WeiXinUserExtension model = GetWeiXinUserExtension();

                if (Session["User"] == null)
                {
                    Session["User"] = GetWeiXinUserExtension();
                    Response.Write("<script defer>location.reload();</script>");
                }
            }
        }

        public WeiXinUserExtension GetWeiXinUserExtension()
        {
            string name = "";
            string pass = "";

            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IWeiXinUserService weiXinUserService = container.Resolve<IWeiXinUserService>();

            if (CookieHelp.GetCookie("UserName") != null && !"".Equals(CookieHelp.GetCookie("UserName")))
            {
                name = CookieHelp.GetCookie("UserName");
            }

            WeiXinUserExtension model = new WeiXinUserExtension();

            if (Session["User"] != null && !Session["User"].Equals(""))
            {
                return Session["User"] as WeiXinUserExtension;
            }
            else if (Session["UserId"] != null)
            {
                string userId = HttpContext.Current.Session["UserId"].ToString();

                model = weiXinUserService.GetWeiXinUserExtensionById(userId);

                if (model != null)
                {
                    Session.Add("User", model);
                }
                else
                {
                    Response.Redirect("login.aspx");
                }

                return model;
            }
            else if (name != "")
            {
                if (CookieHelp.GetDESEncryptedCookieValue("UserPwd") != null && !"".Equals(CookieHelp.GetDESEncryptedCookieValue("UserPwd")))
                {
                    pass = DEncryptHelp.Encrypt(CookieHelp.GetDESEncryptedCookieValue("UserPwd"), 1);
                }

                model = weiXinUserService.GetWeiXinUserExtensionByUserNameAndPassword(name, pass);

                if (model != null)
                {
                    Session.Add("User", model);
                    return model;
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }
            else
            {
                Response.Redirect("login.aspx");
            }

            return null;
        }

        public MobileUser GetMobileUser()
        {
            return null;
        }
    }
}
