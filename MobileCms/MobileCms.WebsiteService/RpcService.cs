using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using MobileCms.Common;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.WebsiteService
{
    public class RpcService : IRpcService
    {
        public void SendRpcCommand(string companyCode, string methodName, List<XmlRpcParamenter> xmlRpcParamenters)
        {
            try
            {
                string content = RpcXmlHelp.createRpcXmlDoc(methodName, xmlRpcParamenters);

                string rpcHost = ConfigHelp.GetConfigString("RpcHost");
                string rpcPort = ConfigHelp.GetConfigString("RpcPort");
                string rpcAppName = ConfigHelp.GetConfigString("RpcAppName");

                string url = rpcHost + ":" + rpcPort + "/" + rpcAppName;

                string contentType = "text/xml";

                HttpWebResponse httpWebResponse = HttpWebRequestHelp.CreatePostHttpResponse(url, contentType, content, null, null, Encoding.UTF8, null, companyCode);
                Stream reader = httpWebResponse.GetResponseStream();

                StreamReader sr = new StreamReader(reader, Encoding.UTF8);
                string result = sr.ReadToEnd();
                reader.Close();
            }
            catch (Exception)
            {
                
            }
        }

        public void runCmdUpdateApp(string ownerUri, string companyCode)
        {
            /*runCmd*/
            List<XmlRpcParamenter> xmlRpcParamenters = new List<XmlRpcParamenter>();

            XmlRpcParamenter xmlRpcParamenter1 = new XmlRpcParamenter();
            xmlRpcParamenter1.ParaName = "mobileRpcParamenter1";
            xmlRpcParamenter1.ParaType = "String";
            xmlRpcParamenter1.ParaValue = "UpdateApp";
            xmlRpcParamenter1.ParaSort = 1;
            xmlRpcParamenters.Add(xmlRpcParamenter1);

            XmlRpcParamenter xmlRpcParamenter2 = new XmlRpcParamenter();
            xmlRpcParamenter2.ParaName = "mobileRpcParamenter2";
            xmlRpcParamenter2.ParaType = "String";
            xmlRpcParamenter2.ParaValue = ownerUri;
            xmlRpcParamenter2.ParaSort = 2;
            xmlRpcParamenters.Add(xmlRpcParamenter2);

            SendRpcCommand(companyCode, "runCmd", xmlRpcParamenters);
        }

        public void runCmdUpdateContact(string ownerUri, string companyCode)
        {
            /*runCmd*/
            List<XmlRpcParamenter> xmlRpcParamenters = new List<XmlRpcParamenter>();

            XmlRpcParamenter xmlRpcParamenter1 = new XmlRpcParamenter();
            xmlRpcParamenter1.ParaName = "mobileRpcParamenter1";
            xmlRpcParamenter1.ParaType = "String";
            xmlRpcParamenter1.ParaValue = "UpdateContact";
            xmlRpcParamenter1.ParaSort = 1;
            xmlRpcParamenters.Add(xmlRpcParamenter1);

            XmlRpcParamenter xmlRpcParamenter2 = new XmlRpcParamenter();
            xmlRpcParamenter2.ParaName = "mobileRpcParamenter2";
            xmlRpcParamenter2.ParaType = "String";
            xmlRpcParamenter2.ParaValue = ownerUri;
            xmlRpcParamenter2.ParaSort = 2;
            xmlRpcParamenters.Add(xmlRpcParamenter2);

            SendRpcCommand(companyCode, "runCmd", xmlRpcParamenters);
        }
    }
}
