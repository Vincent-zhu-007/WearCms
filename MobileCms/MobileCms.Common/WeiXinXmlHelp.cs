using System;
using System.IO;
using System.Xml;

namespace MobileCms.Common
{
    public class WeiXinXmlHelp
    {
        public static string CreateWeiXinReplyTextXmlDoc(string _fromUserName, string _toUserName, string _createTime, string _content)
        {
            string result = "";

            XmlDocument doc = new XmlDocument();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string xmlFilePath = basePath + "XmlTemplete\\wxreplytext.xml";

            doc.Load(xmlFilePath);

            XmlNode rootNode = doc.SelectSingleNode("xml");
            foreach (XmlNode xmlNode in rootNode.ChildNodes)
            {
                if (xmlNode.Name == "ToUserName")
                {
                    xmlNode.InnerText = _fromUserName;
                }
                else if (xmlNode.Name == "FromUserName")
                {
                    xmlNode.InnerText = _toUserName;
                }
                else if (xmlNode.Name == "CreateTime")
                {
                    xmlNode.InnerText = _createTime;
                }
                else if (xmlNode.Name == "MsgType")
                {
                    
                }
                else if (xmlNode.Name == "Content")
                {
                    xmlNode.InnerText = _content;
                }
            }

            result = ConvertXmlToString(doc);

            return result;
        }

        public static string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        }
    }
}
