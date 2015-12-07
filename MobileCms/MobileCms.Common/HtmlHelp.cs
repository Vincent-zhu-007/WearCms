using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobileCms.Container;
using MobileCms.Website;

namespace MobileCms.Common
{
    public class HtmlHelp
    {
        public static string InitDdlOptionHtml(string category, string companyCode)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            Dictionary<string, string> dic = codeService.GetCodeCacheByCategoryFromServer(category, companyCode);

            sb.Append("<option value=\"\"></option>");

            foreach (KeyValuePair<string, string> m in dic)
            {
                sb.Append("<option value=\"" + m.Key + "\">" + m.Value + "</option>");
            }

            return sb.ToString();
        }

        public static string InitDdlOptionSelectHtml(string category, string codeName, string companyCode)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            Dictionary<string, string> dic = codeService.GetCodeCacheByCategoryFromServer(category, companyCode);

            sb.Append("<option value=\"\"></option>");

            foreach (KeyValuePair<string, string> m in dic)
            {
                if (m.Key.Equals(codeName))
                {
                    sb.Append("<option value=\"" + m.Key + "\" selected=\"selected\">" + m.Value + "</option>");
                }
                else
                {
                    sb.Append("<option value=\"" + m.Key + "\">" + m.Value + "</option>");
                }
            }

            return sb.ToString();
        }
    }
}
