using System;

namespace MobileCms.Common
{
    public class WeiXinSignatureHelp
    {
        public static bool CheckWeiXinSignature(string token, string timestamp, string nonce, string signature)
        {
            bool result = false;

            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);
            string code = string.Join("", ArrTmp);

            string tempSignature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(code, "SHA1").ToLower();

            if (tempSignature == signature)
            {
                result = true;
            }

            return result;
        }
    }
}
