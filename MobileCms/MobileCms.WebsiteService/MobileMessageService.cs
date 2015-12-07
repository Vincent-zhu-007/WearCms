using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;

namespace MobileCms.WebsiteService
{
    public class MobileMessageService : IMobileMessageService
    {
        public DataTable GetListByPage(string companyCode, string userName, int start, int limit, out int total)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                StringBuilder queryCountSql = new StringBuilder();
                queryCountSql.Append(@"SELECT a.Id FROM DataContext.MobileMessage as a");
                queryCountSql.Append(" JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as b");
                queryCountSql.Append(" ON a.OwnerUri = b.OwnerUri");
                queryCountSql.Append(" WHERE b.CompanyCode = @companyCode");
                
                if (!String.IsNullOrEmpty(userName))
                {
                    queryCountSql.Append(" AND b.UserName = @userName");
                }

                ObjectQuery<DbDataRecord> queryCount = new ObjectQuery<DbDataRecord>(queryCountSql.ToString(), dataContext);
                queryCount.Parameters.Add(new ObjectParameter("companyCode", companyCode));
                
                if (!String.IsNullOrEmpty(userName))
                {
                    queryCount.Parameters.Add(new ObjectParameter("userName", userName));
                }

                total = queryCount.Count();

                int totalPages = 0;

                if (total > 0 && limit > 0)
                {
                    totalPages = (int)Math.Ceiling((float)total / (float)limit);
                }
                else
                {
                    totalPages = 0;
                }

                if (start > totalPages)
                {
                    start = totalPages;
                }
                if (start < 1)
                {
                    start = 1;
                }

                start = limit * start - limit;

                StringBuilder querySql = new StringBuilder();
                querySql.Append(@"SELECT a.Id, a.ProcessStatus, a.Status, a.CreateTime as CreateTime, b.UserName, b.DisplayName FROM DataContext.MobileMessage as a");
                querySql.Append(" JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as b");
                querySql.Append(" ON a.OwnerUri = b.OwnerUri");
                querySql.Append(" WHERE b.CompanyCode = @companyCode");
                
                if (!String.IsNullOrEmpty(userName))
                {
                    querySql.Append(" AND b.UserName = @userName");
                }

                querySql.Append(" ORDER BY a.CreateTime DESC");

                querySql.Append(@" SKIP @skip LIMIT @limit");

                ObjectQuery<DbDataRecord> query = new ObjectQuery<DbDataRecord>(querySql.ToString(), dataContext);
                query.Parameters.Add(new ObjectParameter("companyCode", companyCode));
                
                if (!String.IsNullOrEmpty(userName))
                {
                    query.Parameters.Add(new ObjectParameter("userName", userName));
                }

                query.Parameters.Add(new ObjectParameter("skip", start));
                query.Parameters.Add(new ObjectParameter("limit", limit));

                List<Code> codes = codeService.GetCodeCache(companyCode);

                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("UserName");
                dt.Columns.Add("DisplayName");
                dt.Columns.Add("ProcessStatus");
                dt.Columns.Add("CreateTime");

                foreach (DbDataRecord rec in query)
                {
                    Code processStatus = codes.Where(m => m.Category == "MOBILEMESSAGEPROCESSSTATUS" && m.CodeName == rec["ProcessStatus"].ToString()).FirstOrDefault();

                    string createTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", rec["CreateTime"]);

                    DataRow dr = dt.NewRow();
                    dr["Id"] = rec["Id"].ToString();
                    dr["UserName"] = rec["UserName"].ToString();
                    dr["DisplayName"] = rec["DisplayName"].ToString();
                    dr["ProcessStatus"] = processStatus == null ? "" : processStatus.Description;
                    dr["CreateTime"] = createTime;

                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        public MobileMessage GetById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileMessage.Where(m => m.Id == id).FirstOrDefault();
            }
        }

        public string CreateMobileMessageFromServer(string companyCode, string ownerUri, string targetUris, string messageContent)
        {
            string resultData = "";

            JObject jObject = new JObject();
            jObject.Add("ownerUri", ownerUri);
            jObject.Add("targetUris", targetUris);
            jObject.Add("content", messageContent);

            string pocServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string pocServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string pocServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string pocServerPart1 = "mobile.mobilemessage.createmobilemessagejson";
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
                    bool isContainData = resultJsonObject.Properties().Any(m => m.Name == "data");
                    if (isContainData)
                    {
                        resultData = resultJsonObject.Property("data").Value.ToString();
                    }
                }
                else
                {
                    bool isContainMessage = resultJsonObject.Properties().Any(m => m.Name == "message");
                    if (isContainMessage)
                    {
                        resultData = resultJsonObject.Property("message").Value.ToString();
                    }
                }
            }

            return resultData;
        }

        public string EditMobileMessageFromServer(string companyCode, string ownerUri, string id, string targetUris, string messageContent)
        {
            string resultData = "";

            JObject jObject = new JObject();
            jObject.Add("ownerUri", ownerUri);
            jObject.Add("id", id);
            jObject.Add("targetUris", targetUris);
            jObject.Add("content", messageContent);

            string pocServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string pocServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string pocServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string pocServerPart1 = "mobile.mobilemessage.editmobilemessagejson";
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
                    bool isContainData = resultJsonObject.Properties().Any(m => m.Name == "data");
                    if (isContainData)
                    {
                        resultData = resultJsonObject.Property("data").Value.ToString();
                    }
                }
                else
                {
                    bool isContainMessage = resultJsonObject.Properties().Any(m => m.Name == "message");
                    if (isContainMessage)
                    {
                        resultData = resultJsonObject.Property("message").Value.ToString();
                    }
                }
            }

            return resultData;
        }

        public string SendMobileMessageFromServer(string companyCode, string id)
        {
            string resultData = "";

            JObject jObject = new JObject();
            jObject.Add("id", id);
            
            string pocServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string pocServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string pocServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string pocServerPart1 = "mobile.mobilemessage.sendmobilemessagejson";
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
                    bool isContainData = resultJsonObject.Properties().Any(m => m.Name == "data");
                    if (isContainData)
                    {
                        resultData = resultJsonObject.Property("data").Value.ToString();
                    }
                }
                else
                {
                    bool isContainMessage = resultJsonObject.Properties().Any(m => m.Name == "message");
                    if (isContainMessage)
                    {
                        resultData = resultJsonObject.Property("message").Value.ToString();
                    }
                }
            }

            return resultData;
        }

        public string DeleteMobileMessageFromServer(string companyCode, string ownerUri, string id)
        {
            string resultData = "";

            JObject jObject = new JObject();
            jObject.Add("ownerUri", ownerUri);
            jObject.Add("id", id);
            
            string pocServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string pocServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string pocServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string pocServerPart1 = "mobile.mobilemessage.deletemobilemessagejson";
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
                    bool isContainData = resultJsonObject.Properties().Any(m => m.Name == "data");
                    if (isContainData)
                    {
                        resultData = resultJsonObject.Property("data").Value.ToString();
                    }
                }
                else
                {
                    bool isContainMessage = resultJsonObject.Properties().Any(m => m.Name == "message");
                    if (isContainMessage)
                    {
                        resultData = resultJsonObject.Property("message").Value.ToString();
                    }
                }
            }

            return resultData;
        }
    }
}