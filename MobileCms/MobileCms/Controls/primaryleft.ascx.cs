using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MobileCms.Common;
using MobileCms.Data;
using Newtonsoft.Json.Linq;

namespace MobileCms.Controls
{
    public partial class primaryleft : System.Web.UI.UserControl
    {
        public string userName = "";
        public string webSiteMenuTreeHtml = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            WeiXinUserExtension weiXinUserExtension = new MobileCms.Common.BasePageMember().GetWeiXinUserExtension();

            if (weiXinUserExtension != null)
            {
                userName = weiXinUserExtension.UserName;

                if (!String.IsNullOrEmpty(weiXinUserExtension.DisplayName))
                {
                    userName = weiXinUserExtension.DisplayName;
                }

                if (userName.Length >= 7)
                {
                    userName = userName.Substring(0, 7) + "...";
                }

                JObject jsonObjectPara = new JObject();
                jsonObjectPara.Add("webSiteRole", weiXinUserExtension.WebSiteRole);

                string mobileServerHost = ConfigHelp.GetConfigString("MobileServerHost");
                string mobileServerPort = ConfigHelp.GetConfigString("MobileServerPort");
                string mobileServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
                string globalPart = ConfigHelp.GetConfigString("GlobalPart");
                string url = mobileServerHost + ":" + mobileServerPort + "/" + mobileServerAppName + "/mobile.websitemenu.initwebsitemenujson/" + globalPart;

                string contentType = "text/json;charset=UTF-8";
                string content = jsonObjectPara.ToString();
                HttpWebResponse httpWebResponse = HttpWebRequestHelp.CreateJsonPostHttpResponse(url, contentType, content, null, null, Encoding.UTF8, null);

                Stream reader = httpWebResponse.GetResponseStream();
                StreamReader sr = new StreamReader(reader, Encoding.UTF8);
                string strJson = sr.ReadToEnd();
                reader.Close();

                webSiteMenuTreeHtml = InitWebSiteMenuTree(strJson);
            }
        }

        private string InitWebSiteMenuTree(string json)
        {
            string html = "";

            JArray jsonArray = JArray.Parse(json);

            html = BulidWebSiteMenuTree(jsonArray);

            return html;
        }

        private string BulidWebSiteMenuTree(JArray jsonArray)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            int i = 0;

            foreach (JObject jsonObject in jsonArray)
            {
                i += 1;

                string text = "";
                bool isContainText = jsonObject.Properties().Any(m => m.Name == "text");
                if (isContainText)
                {
                    text = jsonObject.Property("text").Value.ToString();
                }

                sb.Append("<dt class=\"dt" + i + " png_bg\">" + text + "</dt>");

                string children = "";
                bool isContainChildren = jsonObject.Properties().Any(m => m.Name == "children");
                if (isContainChildren)
                {
                    sb.Append("<dd>");
                    sb.Append("<ul>");

                    children = jsonObject.Property("children").Value.ToString();

                    JArray childrenJsonArray = JArray.Parse(children);

                    foreach (JObject childrenJsonObject in childrenJsonArray)
                    {
                        string childrenText = "";
                        string url = "";

                        bool isContainChildrenText = childrenJsonObject.Properties().Any(m => m.Name == "text");
                        if (isContainChildrenText)
                        {
                            childrenText = childrenJsonObject.Property("text").Value.ToString();
                        }

                        string attributes = "";
                        bool isContainAttributes = childrenJsonObject.Properties().Any(m => m.Name == "attributes");
                        if (isContainAttributes)
                        {
                            attributes = childrenJsonObject.Property("attributes").Value.ToString();

                            JObject attributesJsonObject = JObject.Parse(attributes);

                            bool isContainUrl = attributesJsonObject.Properties().Any(m => m.Name == "url");
                            if (isContainUrl)
                            {
                                url = attributesJsonObject.Property("url").Value.ToString();
                            }
                        }

                        sb.Append("<li>");
                        sb.Append("<a href=\"" + url + "\">");
                        sb.Append("<span class=\"png_bg\"></span>");

                        string rawUrl = Request.RawUrl;

                        string urlAddress = "";
                        int index = rawUrl.LastIndexOf("/");
                        if (index != -1)
                        {
                            urlAddress = rawUrl.Substring(index + 1);
                        }

                        int index2 = urlAddress.LastIndexOf("?");
                        if (index2 != -1)
                        {
                            urlAddress = urlAddress.Substring(0, index2);
                        }

                        if (url.Equals(urlAddress))
                        {
                            sb.Append("<b class=\"on\">" + childrenText + "</b>");
                        }
                        else
                        {
                            sb.Append("<b class=\"\">" + childrenText + "</b>");
                        }

                        sb.Append("</a>");
                        sb.Append("</li>");
                    }

                    sb.Append("</ul>");
                    sb.Append("</dd>");
                }
            }

            return sb.ToString();
        }
    }
}