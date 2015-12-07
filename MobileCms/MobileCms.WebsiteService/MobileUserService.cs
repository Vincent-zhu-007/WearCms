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
    public class MobileUserService : IMobileUserService
    {
        public DataTable GetMobileUsersByPage(string companyCode, string userName, int start, int limit, out int total)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                StringBuilder queryCountSql = new StringBuilder();
                queryCountSql.Append(@"SELECT a.Id FROM OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as a");
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
                querySql.Append(@"SELECT a.Id, a.OwnerUri, a.UserName, a.DisplayName, a.MobilePhone, a.CompanyCode, a.OrgStructure, a.Status, a.CreateTime, a.Gender, a.Birthday, a.MeiNo as MeiNo FROM OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as a");
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

        public MobileUser GetMobileUserById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUser.Where(m => m.Id == id).FirstOrDefault();
            }
        }

        public MobileUser GetMobileUserByUserName(string userName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUser.Where(m => m.UserName == userName && m.Status == "Y").FirstOrDefault();
            }
        }

        public MobileUser GetMobileUserByOwnerUri(string ownerUri)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUser.Where(m => m.OwnerUri == ownerUri && m.Status == "Y").FirstOrDefault();
            }
        }

        public List<MobileUser> GetMobileUserByOrgStructure(string orgStructure)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUser.Where(m => m.OrgStructure == orgStructure && m.Status == "Y").ToList();
            }
        }

        public MobileUser GetMobileUserByUserNameAndPassword(string userName, string password)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUser.Where(m => m.UserName == userName && m.Password == password && m.Status == "Y").FirstOrDefault();
            }
        }

        public void Create(MobileUser model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                dataContext.AddToMobileUser(model);
                dataContext.SaveChanges();
            }
        }

        public void Update(MobileUser model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileUser entity = dataContext.MobileUser.Where(m => m.Id == model.Id).FirstOrDefault();

                entity.DisplayName = model.DisplayName;
                entity.Password = model.Password;
                entity.Status = model.Status;
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
                MobileUser entity = dataContext.MobileUser.Where(m => m.Id == id).FirstOrDefault();

                if (entity != null)
                {
                    dataContext.MobileUser.DeleteObject(entity);
                    dataContext.SaveChanges();
                }
            }
        }

        public List<MobileUser> GetMobileUserByCompanyCode(string companyCode)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUser.Where(m => m.CompanyCode == companyCode).OrderBy(m => m.UserName).ToList();
            }
        }

        public string LockMobileFromServer(string companyCode, string id)
        {
            string resultData = "";

            JObject jObject = new JObject();
            jObject.Add("id", id);

            string pocServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string pocServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string pocServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string pocServerPart1 = "mobile.mobileuser.sendmobilelockjson";
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

        public string ClearMobileFromServer(string companyCode, string id)
        {
            string resultData = "";

            JObject jObject = new JObject();
            jObject.Add("id", id);

            string pocServerHost = ConfigHelp.GetConfigString("MobileServerHost");
            string pocServerPort = ConfigHelp.GetConfigString("MobileServerPort");
            string pocServerAppName = ConfigHelp.GetConfigString("MobileServerAppName");
            string pocServerPart1 = "mobile.mobileuser.sendmobileclearjson";
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

        public Dictionary<string, string> GetMobileUserDicByCompanyCode(string companyCode)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                List<MobileUser> mobileUsers = dataContext.MobileUser.Where(m => m.CompanyCode == companyCode).OrderBy(m => m.UserName).ToList();

                Dictionary<string, string> dic = new Dictionary<string, string>();

                foreach (MobileUser m in mobileUsers)
                {
                    dic.Add(m.OwnerUri, m.DisplayName);
                }

                return dic;
            }
        }
    }
}