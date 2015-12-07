using System;

namespace MobileCms.Data
{
    [Serializable]
    public class WeiXinAuthorization
    {
        public WeiXinAuthorization() { }

        private string accessToken;

        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }
        private string expiresIn;

        public string ExpiresIn
        {
            get { return expiresIn; }
            set { expiresIn = value; }
        }
        private string refreshToken;

        public string RefreshToken
        {
            get { return refreshToken; }
            set { refreshToken = value; }
        }
        private string openId;

        public string OpenId
        {
            get { return openId; }
            set { openId = value; }
        }
        private string scope;

        public string Scope
        {
            get { return scope; }
            set { scope = value; }
        }
        private string unionId;

        public string UnionId
        {
            get { return unionId; }
            set { unionId = value; }
        }
    }
}
