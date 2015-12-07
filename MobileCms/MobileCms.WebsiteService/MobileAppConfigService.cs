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
    public class MobileAppConfigService : IMobileAppConfigService
    {
        public DataTable GetListByPage(string companyCode, string description, int start, int limit, out int total)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                StringBuilder queryCountSql = new StringBuilder();
                queryCountSql.Append(@"SELECT a.Id FROM DataContext.MobileAppConfig as a");
                queryCountSql.Append(" WHERE 1 = 1");

                if (!String.IsNullOrEmpty(description))
                {
                    queryCountSql.Append(" AND a.Description like '%" + description + "%'");
                }

                ObjectQuery<DbDataRecord> queryCount = new ObjectQuery<DbDataRecord>(queryCountSql.ToString(), dataContext);

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
                querySql.Append(@"SELECT a.Id, a.CodeName, a.Description, a.PackageName, a.Status, a.CreateTime FROM DataContext.MobileAppConfig as a");
                querySql.Append(" WHERE 1 = 1");

                if (!String.IsNullOrEmpty(description))
                {
                    querySql.Append(" AND a.Description like '%" + description + "%'");
                }

                querySql.Append(" ORDER BY a.CreateTime DESC");

                querySql.Append(@" SKIP @skip LIMIT @limit");

                ObjectQuery<DbDataRecord> query = new ObjectQuery<DbDataRecord>(querySql.ToString(), dataContext);

                query.Parameters.Add(new ObjectParameter("skip", start));
                query.Parameters.Add(new ObjectParameter("limit", limit));

                List<Code> codes = codeService.GetCodeCache(companyCode);

                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("CodeName");
                dt.Columns.Add("Description");
                dt.Columns.Add("PackageName");
                dt.Columns.Add("Status");
                dt.Columns.Add("CreateTime");
                
                foreach (DbDataRecord rec in query)
                {
                    Code statusCode = codes.Where(m => m.Category == "STATUS" && m.CodeName == rec["Status"].ToString()).FirstOrDefault();

                    string createTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", rec["CreateTime"]);

                    DataRow dr = dt.NewRow();
                    dr["Id"] = rec["Id"].ToString();
                    dr["CodeName"] = rec["CodeName"].ToString();
                    dr["Description"] = rec["Description"].ToString();
                    dr["PackageName"] = rec["PackageName"].ToString();
                    dr["Status"] = statusCode == null ? "" : statusCode.Description;
                    dr["CreateTime"] = createTime;
                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        public MobileAppConfig GetById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileAppConfig.Where(m => m.Id == id).FirstOrDefault();
            }
        }

        public void Create(MobileAppConfig model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                dataContext.AddToMobileAppConfig(model);
                dataContext.SaveChanges();
            }
        }

        public void Update(MobileAppConfig model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileAppConfig entity = dataContext.MobileAppConfig.Where(m => m.Id == model.Id).FirstOrDefault();

                entity.Description = model.Description;
                entity.PackageName = model.PackageName;
                entity.Updator = model.Updator;
                entity.UpdateTime = model.UpdateTime;

                dataContext.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileAppConfig entity = dataContext.MobileAppConfig.Where(m => m.Id == id).FirstOrDefault();

                if (entity != null)
                {
                    dataContext.MobileAppConfig.DeleteObject(entity);
                    dataContext.SaveChanges();
                }
            }
        }

        public MobileAppConfig GetMobileAppConfigByCodeName(string codeName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileAppConfig.Where(m => m.CodeName == codeName).FirstOrDefault();
            }
        }

        public Dictionary<string, string> GetMobileAppConfigCacheFromServer(string companyCode)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            JObject jObject = new JObject();
            
            string pocServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string pocServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string pocServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string pocServerPart1 = "mobile.mobileappconfig.getmobileappconfigcachejson";
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

        public string ClearMobileAppConfigCacheFromServer(string companyCode)
        {
            string strData = "";

            JObject jObject = new JObject();

            string pocServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string pocServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string pocServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string pocServerPart1 = "mobile.mobileappconfig.clearmobileappconfigcachejson";
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
                        strData = resultJsonObject.Property("data").Value.ToString();
                    }

                }
            }

            return strData;
        }
    }
}
