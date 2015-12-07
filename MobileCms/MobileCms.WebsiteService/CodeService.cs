using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MobileCms.Common;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;

namespace MobileCms.WebsiteService
{
    public class CodeService : ICodeService
    {
        public List<Code> GetCodeCache(string companyCode)
        {
            List<Code> list = new List<Code>();

            string mobileServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string mobileServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string mobileServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string mobileServerPart1 = "mobile.systemcode.getcode";
            string mobileServerPart2 = ConfigHelp.GetConfigString("GlobalPart");

            string url = mobileServerHost + ":" + mobileServerPort + "/" + mobileServerAppName + "/" + mobileServerPart1 + "/" + mobileServerPart2;
            string content = "";
            string contentType = "text/json";

            HttpWebResponse httpWebResponse = HttpWebRequestHelp.CreatePostHttpResponse(url, contentType, content, null, null, System.Text.Encoding.UTF8, null, companyCode);
            Stream reader = httpWebResponse.GetResponseStream();
            StreamReader sr = new StreamReader(reader, Encoding.UTF8);
            string jsonData = sr.ReadToEnd();
            reader.Close();

            if (!String.IsNullOrEmpty(jsonData))
            {
                JObject resultJsonObject = JObject.Parse(jsonData);

                string result = "";
                bool isContainResult = resultJsonObject.Properties().Any(m => m.Name == "result");
                if (isContainResult)
                {
                    result = resultJsonObject.Property("result").Value.ToString();
                }

                if (result.Equals("1"))
                {
                    string strData = "";
                    bool isContainData = resultJsonObject.Properties().Any(m => m.Name == "data");
                    if (isContainData)
                    {
                        strData = resultJsonObject.Property("data").Value.ToString();
                    }

                    if (!String.IsNullOrEmpty(strData))
                    {
                        JArray jsonArray = JArray.Parse(strData);

                        foreach (JObject jsonObject in jsonArray)
                        {
                            Code code = new Code();

                            string codeName = "";
                            bool isContainCodeName = jsonObject.Properties().Any(m => m.Name == "codeName");
                            if (isContainCodeName)
                            {
                                codeName = jsonObject.Property("codeName").Value.ToString();
                                code.CodeName = codeName;
                            }

                            string description = "";
                            bool isContainDescription = jsonObject.Properties().Any(m => m.Name == "description");
                            if (isContainDescription)
                            {
                                description = jsonObject.Property("description").Value.ToString();
                                code.Description = description;
                            }

                            string category = "";
                            bool isContainCategory = jsonObject.Properties().Any(m => m.Name == "category");
                            if (isContainCategory)
                            {
                                category = jsonObject.Property("category").Value.ToString();
                                code.Category = category;
                            }

                            list.Add(code);
                        }
                    }
                }
            }

            return list;
        }

        public Dictionary<string, string> GetCodeCacheByCategoryFromServer(string category, string companyCode)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            JObject jObject = new JObject();
            jObject.Add("category", category);

            string pocServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string pocServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string pocServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string pocServerPart1 = "mobile.systemcode.getcodebycategory";
            string pocServerPart2 = ConfigHelp.GetConfigString("GlobalPart");

            string url = pocServerHost + ":" + pocServerPort + "/" + pocServerAppName + "/" + pocServerPart1 + "/" + pocServerPart2;
            string content = jObject.ToString();
            string contentType = "text/json";

            HttpWebResponse httpWebResponse = HttpWebRequestHelp.CreatePostHttpResponse(url, contentType, content, null, null, System.Text.Encoding.UTF8, null, companyCode);
            Stream reader = httpWebResponse.GetResponseStream();
            StreamReader sr = new StreamReader(reader, Encoding.UTF8);
            string jsonData = sr.ReadToEnd();
            reader.Close();

            if (!String.IsNullOrEmpty(jsonData))
            {
                JObject resultJsonObject = JObject.Parse(jsonData);

                string result = "";
                bool isContainResult = resultJsonObject.Properties().Any(m => m.Name == "result");
                if (isContainResult)
                {
                    result = resultJsonObject.Property("result").Value.ToString();
                }

                if (result.Equals("1"))
                {
                    string strData = "";
                    bool isContainData = resultJsonObject.Properties().Any(m => m.Name == "data");
                    if (isContainData)
                    {
                        strData = resultJsonObject.Property("data").Value.ToString();
                    }

                    if (!String.IsNullOrEmpty(strData))
                    {
                        JArray jsonArray = JArray.Parse(strData);

                        foreach (JObject jsonObject in jsonArray)
                        {
                            string codeName = "";
                            bool isContainCodeName = jsonObject.Properties().Any(m => m.Name == "codeName");
                            if (isContainCodeName)
                            {
                                codeName = jsonObject.Property("codeName").Value.ToString();
                            }

                            string description = "";
                            bool isContainDescription = jsonObject.Properties().Any(m => m.Name == "description");
                            if (isContainDescription)
                            {
                                description = jsonObject.Property("description").Value.ToString();
                            }

                            if (!String.IsNullOrEmpty(codeName) && !String.IsNullOrEmpty(description))
                            {
                                dic.Add(codeName, description);
                            }
                        }
                    }
                }
            }

            return dic;
        }
    }
}
