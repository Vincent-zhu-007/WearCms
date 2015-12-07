using System;
using System.Web;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.Common
{
    public class BasePageWeiXinMember : BasePage
    {
        public BasePageWeiXinMember()
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
                Page.Title = "诺维小天使";

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

            if (CookieHelp.GetCookie("WeiXinUserName") != null && !"".Equals(CookieHelp.GetCookie("WeiXinUserName")))
            {
                name = CookieHelp.GetCookie("WeiXinUserName");
            }

            WeiXinUserExtension model = new WeiXinUserExtension();

            if (Session["WeiXinUser"] != null && !Session["WeiXinUser"].Equals(""))
            {
                return Session["WeiXinUser"] as WeiXinUserExtension;
            }
            else if (Session["WeiXinUserId"] != null)
            {
                string userId = HttpContext.Current.Session["WeiXinUserId"].ToString();

                model = weiXinUserService.GetWeiXinUserExtensionById(userId);

                if (model != null)
                {
                    Session.Add("WeiXinUser", model);
                }
                else
                {
                    Response.Redirect("wxlogin.aspx");
                }

                return model;
            }
            else if (name != "")
            {
                if (CookieHelp.GetDESEncryptedCookieValue("WeiXinUserPwd") != null && !"".Equals(CookieHelp.GetDESEncryptedCookieValue("WeiXinUserPwd")))
                {
                    pass = DEncryptHelp.Encrypt(CookieHelp.GetDESEncryptedCookieValue("WeiXinUserPwd"), 1);
                }

                model = weiXinUserService.GetWeiXinUserExtensionByUserNameAndPassword(name, pass);

                if (model != null)
                {
                    Session.Add("WeiXinUser", model);
                    return model;
                }
                else
                {
                    Response.Redirect("wxlogin.aspx");
                }
            }
            else
            {
                Response.Redirect("wxlogin.aspx");
            }

            return null;
        }
    }
}
