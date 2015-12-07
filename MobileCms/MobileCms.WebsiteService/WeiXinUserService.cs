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
    public class WeiXinUserService : IWeiXinUserService
    {
        public DataTable GetWeiXinUsersByPage(string companyCode, string userName, int start, int limit, out int total)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                StringBuilder queryCountSql = new StringBuilder();
                queryCountSql.Append(@"SELECT a.Id FROM OFTYPE(DataContext.WeiXinUser, MobileCms.Data.WeiXinUserExtension) as a");
                queryCountSql.Append(" WHERE a.CompanyCode = @companyCode");

                if (!String.IsNullOrEmpty(userName))
                {
                    queryCountSql.Append(" AND a.UserName = @userName");
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
                querySql.Append(@"SELECT a.Id, a.OwnerUri, a.UserName, a.DisplayName, a.MobilePhone, a.CompanyCode, a.OrgStructure, a.Status, a.CreateTime, a.Gender, a.Birthday, a.MeiNo as MeiNo FROM OFTYPE(DataContext.WeiXinUser, MobileCms.Data.WeiXinUserExtension) as a");
                querySql.Append(" WHERE a.CompanyCode = @companyCode");

                if (!String.IsNullOrEmpty(userName))
                {
                    querySql.Append(" AND a.UserName = @userName");
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
                dt.Columns.Add("OwnerUri");
                dt.Columns.Add("UserName");
                dt.Columns.Add("DisplayName");
                dt.Columns.Add("MobilePhone");
                dt.Columns.Add("CompanyCode");
                dt.Columns.Add("OrgStructure");
                dt.Columns.Add("Status");
                dt.Columns.Add("CreateTime");
                dt.Columns.Add("Gender");
                dt.Columns.Add("Birthday");
                dt.Columns.Add("MeiNo");
                dt.Columns.Add("DisableOption");
                dt.Columns.Add("StatusCode");

                foreach (DbDataRecord rec in query)
                {
                    Code statusCode = codes.Where(m => m.Category == "STATUS" && m.CodeName == rec["Status"].ToString()).FirstOrDefault();

                    Code genderCode = null;
                    if (rec["Gender"] != null && !rec["Gender"].ToString().Equals(""))
                    {
                        genderCode = codes.Where(m => m.Category == "GENDER" && m.CodeName == rec["Gender"].ToString()).FirstOrDefault();
                    }

                    string createTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", rec["CreateTime"]);

                    DataRow dr = dt.NewRow();
                    dr["Id"] = rec["Id"].ToString();
                    dr["OwnerUri"] = rec["OwnerUri"].ToString();
                    dr["UserName"] = rec["UserName"].ToString();
                    dr["DisplayName"] = rec["DisplayName"].ToString();
                    dr["MobilePhone"] = rec["MobilePhone"].ToString();
                    dr["CompanyCode"] = rec["CompanyCode"].ToString();
                    dr["OrgStructure"] = rec["OrgStructure"].ToString();
                    dr["Status"] = statusCode == null ? "" : statusCode.Description;
                    dr["CreateTime"] = createTime;
                    dr["Gender"] = genderCode == null ? "" : genderCode.Description;
                    dr["Birthday"] = rec["Birthday"] == null ? "" : rec["Birthday"].ToString();
                    dr["MeiNo"] = rec["MeiNo"] == null ? "" : rec["MeiNo"].ToString();
                    dr["DisableOption"] = rec["Status"].ToString() == "Y" ? "禁用" : "启用";
                    dr["StatusCode"] = rec["Status"].ToString();

                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        public WeiXinUser GetWeiXinUserById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.WeiXinUser.Where(m => m.Id == id).FirstOrDefault();
            }
        }

        public WeiXinUserExtension GetWeiXinUserExtensionById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.WeiXinUser.OfType<WeiXinUserExtension>().Where(m => m.Id == id).FirstOrDefault();
            }
        }

        public WeiXinUserExtension GetWeiXinUserExtensionByUserName(string userName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.WeiXinUser.OfType<WeiXinUserExtension>().Where(m => m.UserName == userName && m.Status == "Y").FirstOrDefault();
            }
        }

        public WeiXinUser GetWeiXinUserByOwnerUri(string ownerUri)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.WeiXinUser.Where(m => m.OwnerUri == ownerUri && m.Status == "Y").FirstOrDefault();
            }
        }

        public WeiXinUserExtension GetWeiXinUserExtensionByUserNameAndPassword(string userName, string password)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.WeiXinUser.OfType<WeiXinUserExtension>().Where(m => m.UserName == userName && m.Password == password && m.Status == "Y").FirstOrDefault();
            }
        }

        public WeiXinUser GetWeiXinUserByOpenId(string openId)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.WeiXinUser.Where(m => m.OpenId == openId).FirstOrDefault();
            }
        }

        public void Create(WeiXinUserExtension model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                dataContext.AddToWeiXinUser(model);
                dataContext.SaveChanges();
            }
        }

        public string SendMobileListeningFromServer(string openId, string companyCode)
        {
            string resultData = "";

            JObject jObject = new JObject();
            jObject.Add("openId", openId);
            
            string mobileServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string mobileServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string mobileServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string mobileServerPart1 = "mobile.mobileuser.sendmobilelisteningfromfwhjson";
            string mobileServerPart2 = ConfigHelp.GetConfigString("GlobalPart");

            string url = mobileServerHost + ":" + mobileServerPort + "/" + mobileServerAppName + "/" + mobileServerPart1 + "/" + mobileServerPart2;
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
