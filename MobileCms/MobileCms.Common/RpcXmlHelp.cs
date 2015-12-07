using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MobileCms.Data;

namespace MobileCms.Common
{
    public class RpcXmlHelp
    {
        public static string createRpcXmlDoc(String methodName, List<XmlRpcParamenter> paras)
        {
            string result = "";

            XmlDocument doc = new XmlDocument();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string xmlFilePath = basePath + "XmlTemplete\\rpcxmlbase.xml";

            doc.Load(xmlFilePath);

            XmlNodeList methodCallNodes = doc.GetElementsByTagName("methodCall");
            if (methodCallNodes != null && methodCallNodes.Count > 0)
            {
                XmlElement methodCallElement = (XmlElement)methodCallNodes[0];

                XmlElement methodNameElement = doc.CreateElement("methodName");
                methodNameElement.InnerText = methodName;
                methodCallElement.AppendChild(methodNameElement);

                XmlElement paramsElement = doc.CreateElement("params");
                methodCallElement.AppendChild(paramsElement);

                if (paras != null && paras.Count > 0)
                {
                    foreach (XmlRpcParamenter xmlRpcParamenter in paras)
                    {
                        XmlElement paramElement = doc.CreateElement("param");
                        paramsElement.AppendChild(paramElement);

                        XmlElement valueElement = doc.CreateElement("value");
                        paramElement.AppendChild(valueElement);

                        XmlElement paraTypeElement = doc.CreateElement(xmlRpcParamenter.ParaType);
                        paraTypeElement.InnerText = xmlRpcParamenter.ParaValue;
                        valueElement.AppendChild(paraTypeElement);
                    }
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
