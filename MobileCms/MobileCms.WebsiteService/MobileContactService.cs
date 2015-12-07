using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using System.Text;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;

namespace MobileCms.WebsiteService
{
    public class MobileContactService : IMobileContactService
    {
        public DataTable GetListByPage(string companyCode, string userName, string listType, int start, int limit, out int total)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                StringBuilder queryCountSql = new StringBuilder();
                queryCountSql.Append(@"SELECT d.Id FROM OFTYPE(DataContext.WeiXinUser, MobileCms.Data.WeiXinUserExtension) as a");
                queryCountSql.Append(" JOIN DataContext.WeiXinInMobile as b");
                queryCountSql.Append(" ON b.WeiXinOwnerUri = a.OwnerUri");
                queryCountSql.Append(" JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as c");
                queryCountSql.Append(" ON c.OwnerUri = b.MobileOwnerUri");
                queryCountSql.Append(" JOIN DataContext.MobileContact as d");
                queryCountSql.Append(" ON d.OwnerUri = c.OwnerUri");
                queryCountSql.Append(" WHERE d.ListType = @listType");
                queryCountSql.Append(" AND c.CompanyCode = @companyCode");

                if (!String.IsNullOrEmpty(userName))
                {
                    queryCountSql.Append(" AND a.UserName = @userName");
                }

                ObjectQuery<DbDataRecord> queryCount = new ObjectQuery<DbDataRecord>(queryCountSql.ToString(), dataContext);
                queryCount.Parameters.Add(new ObjectParameter("companyCode", companyCode));
                queryCount.Parameters.Add(new ObjectParameter("listType", listType));
                
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
                querySql.Append(@"SELECT d.Id as MobileContactId, a.Status, a.CreateTime, a.UserName as WeiXinUserName, a.DisplayName as WeiXinDisplayName, c.UserName as MobileUserName, c.DisplayName as MobileDisplayName FROM OFTYPE(DataContext.WeiXinUser, MobileCms.Data.WeiXinUserExtension) as a");
                querySql.Append(" JOIN DataContext.WeiXinInMobile as b");
                querySql.Append(" ON b.WeiXinOwnerUri = a.OwnerUri");
                querySql.Append(" JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as c");
                querySql.Append(" ON c.OwnerUri = b.MobileOwnerUri");
                querySql.Append(" JOIN DataContext.MobileContact as d");
                querySql.Append(" ON d.OwnerUri = c.OwnerUri");
                querySql.Append(" WHERE d.ListType = @listType");
                querySql.Append(" AND c.CompanyCode = @companyCode");

                if (!String.IsNullOrEmpty(userName))
                {
                    querySql.Append(" AND a.UserName = @userName");
                }

                querySql.Append(" ORDER BY a.CreateTime DESC");

                querySql.Append(@" SKIP @skip LIMIT @limit");

                ObjectQuery<DbDataRecord> query = new ObjectQuery<DbDataRecord>(querySql.ToString(), dataContext);
                query.Parameters.Add(new ObjectParameter("companyCode", companyCode));
                query.Parameters.Add(new ObjectParameter("listType", listType));
                
                if (!String.IsNullOrEmpty(userName))
                {
                    query.Parameters.Add(new ObjectParameter("userName", userName));
                }

                query.Parameters.Add(new ObjectParameter("skip", start));
                query.Parameters.Add(new ObjectParameter("limit", limit));

                List<Code> codes = codeService.GetCodeCache(companyCode);

                DataTable dt = new DataTable();
                dt.Columns.Add("MobileContactId");
                dt.Columns.Add("WeiXinUserName");
                dt.Columns.Add("WeiXinDisplayName");
                dt.Columns.Add("MobileUserName");
                dt.Columns.Add("MobileDisplayName");
                dt.Columns.Add("Status");
                dt.Columns.Add("CreateTime");

                foreach (DbDataRecord rec in query)
                {
                    Code statusCode = codes.Where(m => m.Category == "STATUS" && m.CodeName == rec["Status"].ToString()).FirstOrDefault();

                    string createTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", rec["CreateTime"]);

                    DataRow dr = dt.NewRow();
                    dr["MobileContactId"] = rec["MobileContactId"].ToString();
                    dr["WeiXinUserName"] = rec["WeiXinUserName"].ToString();
                    dr["WeiXinDisplayName"] = rec["WeiXinDisplayName"].ToString();
                    dr["MobileUserName"] = rec["MobileUserName"].ToString();
                    dr["MobileDisplayName"] = rec["MobileDisplayName"].ToString();
                    dr["Status"] = statusCode == null ? "" : statusCode.Description;
                    dr["CreateTime"] = createTime;

                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        public MobileContact GetMobileContactByOwnerUriAndListType(string ownerUri, string listType)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileContact.Where(m => m.OwnerUri == ownerUri && m.ListType == listType).FirstOrDefault();
            }
        }

        public void Create(MobileContact model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                dataContext.AddToMobileContact(model);
                dataContext.SaveChanges();
            }
        }

        public MobileContact GetById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileContact.Where(m => m.Id == id).FirstOrDefault();
            }
        }

        public void Delete(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileContact entity = dataContext.MobileContact.Where(m => m.Id == id).FirstOrDefault();

                if (entity != null)
                {
                    dataContext.MobileContact.DeleteObject(entity);
                    dataContext.SaveChanges();
                }
            }
        }

        public void Update(MobileContact model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileContact entity = dataContext.MobileContact.Where(m => m.Id == model.Id).FirstOrDefault();

                entity.Etag = model.Etag;
                entity.Status = model.Status;
                entity.Updator = model.Updator;
                entity.UpdateTime = model.UpdateTime;

                dataContext.SaveChanges();
            }
        }

        public MobileContact GetMobileContactByOwnerUriAndListFileName(string ownerUri, string listFileName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileContact.Where(m => m.OwnerUri == ownerUri && m.ListFileName == listFileName).FirstOrDefault();
            }
        }

        public void SendMqttContactList(string ownerUri, string listFileName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();

            IMobileUserService mobileUserService = container.Resolve<IMobileUserService>();
            MobileUser tempMobileUser = mobileUserService.GetMobileUserByOwnerUri(ownerUri);

            string mqttTopic = "host/" + tempMobileUser.UserName + "";
            JObject jsonObjectMqttData = new JObject();
            jsonObjectMqttData.Add("cmd", "contactList");

            IMobileContactMemberService mobileContactMemberService = container.Resolve<IMobileContactMemberService>();
            List<MobileContactMember> newMobileContactMembers = mobileContactMemberService.GetMobileContactMembersByOwnerUriAndListFileName(ownerUri, listFileName);

            JArray JsonArrayMobileContactMember = new JArray();

            if (newMobileContactMembers != null && newMobileContactMembers.Count > 0)
            {
                foreach (MobileContactMember newMobileContactMember in newMobileContactMembers)
                {
                    JObject jsonObjectMobileContactMember = new JObject();

                    jsonObjectMobileContactMember.Add("numButton", newMobileContactMember.NumButton);
                    jsonObjectMobileContactMember.Add("shortNum", newMobileContactMember.ShortNum == null ? "" : newMobileContactMember.ShortNum);
                    jsonObjectMobileContactMember.Add("mobilePhone", newMobileContactMember.MobilePhone);

                    JsonArrayMobileContactMember.Add(jsonObjectMobileContactMember);
                }
            }

            jsonObjectMqttData.Add("data", JsonArrayMobileContactMember.ToString());

            string mqttData = jsonObjectMqttData.ToString();

            MqttHelp.SendMqttMessage(mqttTopic, mqttData);
        }
    }
}
