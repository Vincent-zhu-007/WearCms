using System;
using System.Web;

namespace MobileCms.Common
{
    public class CookieHelp
    {
        public CookieHelp()
        {
        }

        public static void SetCookie(HttpCookie cookie)
        {
            //设置Cookie
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        public static void SetCookie(string key, string valueString)
        {
            //设置加密后的Cookie
            HttpCookie cookie = new HttpCookie(HttpContext.Current.Server.UrlEncode(key), HttpContext.Current.Server.UrlEncode(valueString));
            SetCookie(cookie);
        }

        public static void SetCookie(string key, string valueString, int iexpires)
        {
            //设置加密后的Cookie，并设置Cookie的有效时间
            key = HttpContext.Current.Server.UrlEncode(key);
            valueString = HttpContext.Current.Server.UrlEncode(valueString);
            HttpCookie cookie = new HttpCookie(key, valueString);
            if (iexpires > 0)
            {
                if (iexpires == 1)
                {
                    cookie.Expires = DateTime.MaxValue;
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddSeconds(iexpires);
                }
            }
            SetCookie(cookie);
        }

        public static void SetDESEncryptedCookie(string key, string valueString)
        {
            //设置使用DES加密后的Cookie
            SetCookie(DESEncrypt.Encrypt(key), DESEncrypt.Encrypt(valueString));
        }

        public static void SetDESEncryptedCookie(string key, string valueString, int iexpires)
        {
            //设置使用DES加密后的Cookie，并设置Cookie的有效时间
            SetCookie(DESEncrypt.Encrypt(key), DESEncrypt.Encrypt(valueString), iexpires);
        }

        public static string GetDESEncryptedCookieValue(string key)
        {
            //获取使用DES解密后的Cookie
            string value = GetCookie(DESEncrypt.Encrypt(key));
            if (value != null)
                return DESEncrypt.Decrypt(value);
            else
                return null;
        }

        public static string GetCookie(string key)
        {
            //通过关键字获取Cookie
            HttpCookie cookie = HttpContext.Current.Request.Cookies[HttpContext.Current.Server.UrlEncode(key)];
            if (cookie != null)
                return HttpContext.Current.Server.UrlDecode(cookie.Value);
            else
                return null;
        }

        public static void DeleteCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void DeleteDESCookie(string key)
        {
            DeleteCookie(DESEncrypt.Encrypt(key));
        }
    }
}
