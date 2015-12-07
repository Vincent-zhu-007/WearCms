using System;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for weixinsignature
    /// </summary>
    public class weixinsignature : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string signature = RequestHelp.GetString("signature") == null ? "no signature" : RequestHelp.GetString("signature");
            string timestamp = RequestHelp.GetString("timestamp");
            string nonce = RequestHelp.GetString("nonce");
            string echostr = RequestHelp.GetString("echostr");

            string token = ConfigHelp.GetConfigString("Token");

            bool isPass = WeiXinSignatureHelp.CheckWeiXinSignature(token, timestamp, nonce, signature);

            if (isPass)
            {
                try
                {
                    byte[] byts = new byte[context.Request.InputStream.Length];
                    context.Request.InputStream.Read(byts, 0, byts.Length);
                    string message = System.Text.Encoding.UTF8.GetString(byts);

                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.LoadXml(message);

                    XmlNode rootNode = xmlDoc.SelectSingleNode("xml");

                    string _toUserName = "";
                    string _fromUserName = "";
                    string _msgType = "";
                    string _event = "";
                    string _eventKey = "";

                    foreach (XmlNode xmlNode in rootNode.ChildNodes)
                    {
                        if (xmlNode.Name == "ToUserName")
                        {
                            _toUserName = xmlNode.InnerText;
                        }
                        else if (xmlNode.Name == "FromUserName")
                        {
                            _fromUserName = xmlNode.InnerText;
                        }
                        else if (xmlNode.Name == "MsgType")
                        {
                            _msgType = xmlNode.InnerText;
                        }
                        else if (xmlNode.Name == "Event")
                        {
                            _event = xmlNode.InnerText;
                        }
                        else if (xmlNode.Name == "EventKey")
                        {
                            _eventKey = xmlNode.InnerText;
                        }
                    }

                    ProcessMessage(echostr, _toUserName, _fromUserName, _msgType, _event, _eventKey, context);
                }
                catch (Exception ex)
                {
                    context.Response.Write("");
                    return;
                }
            }
            else
            {
                context.Response.Write("");
                return;
            }
        }

        private void ProcessMessage(string echostr, string _toUserName, string _fromUserName, string _msgType, string _event, string _eventKey, HttpContext context)
        {
            string resultMessage = echostr;

            if (_event == "CLICK")
            {
                if (_eventKey == "listening")
                {
                    DateTime now = DateTime.Now;
                    string _createTime = now.Ticks.ToString();

                    string _content = "";

                    IComponentContainer container = ComponentContainerFactory.CreateContainer();
                    IWeiXinUserService weiXinUserService = container.Resolve<IWeiXinUserService>();

                    WeiXinUser weiXinUser = weiXinUserService.GetWeiXinUserByOpenId(_fromUserName);

                    if (weiXinUser != null)
                    {
                        string message = weiXinUserService.SendMobileListeningFromServer(_fromUserName, weiXinUser.CompanyCode);

                        if (message == "200")
                        {
                            _content = "已经成功发送监听指令，请耐心等待";
                        }
                        else
                        {
                            _content = message;
                        }
                    }
                    else
                    {
                        _content = "请先注册.";
                    }

                    resultMessage = WeiXinXmlHelp.CreateWeiXinReplyTextXmlDoc(_fromUserName, _toUserName, _createTime, _content);
                }
            }

            context.Response.Write(resultMessage);
            return;

            //string log = "ToUserName:" + _toUserName + ",FromUserName:" + _fromUserName + ",MsgType:" + _msgType + ",Event:" + _event + ",EventKey:" + _eventKey;

            //WeiXinMessage weiXinMessage = new WeiXinMessage();
            //weiXinMessage.Id = Guid.NewGuid().ToString();
            //weiXinMessage.Message = log;
            //weiXinMessage.CreateTime = DateTime.Now;

            //IComponentContainer container = ComponentContainerFactory.CreateContainer();
            //IWeiXinMessageService weiXinMessageService = container.Resolve<IWeiXinMessageService>();

            //weiXinMessageService.Create(weiXinMessage);
        }

        private void ProcessListening()
        {
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}