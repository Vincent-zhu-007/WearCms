using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using System.Text;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.WebsiteService
{
    public class MobileAppService : IMobileAppService
    {
        public DataTable GetListByPage(string companyCode, string userName, string listType, int start, int limit, out int total)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                StringBuilder queryCountSql = new StringBuilder();
                queryCountSql.Append(@"SELECT a.Id FROM DataContext.MobileApp as a");
                queryCountSql.Append(" JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as b");
                queryCountSql.Append(" ON a.OwnerUri = b.OwnerUri");
                queryCountSql.Append(" WHERE a.ListType = @listType");
                queryCountSql.Append(" AND b.CompanyCode = @companyCode");

                if (!String.IsNullOrEmpty(userName))
                {
                    queryCountSql.Append(" AND b.UserName = @userName");
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
                querySql.Append(@"SELECT a.Id, a.Status, a.CreateTime, b.UserName, b.DisplayName FROM DataContext.MobileApp as a");
                querySql.Append(" JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as b");
                querySql.Append(" ON a.OwnerUri = b.OwnerUri");
                querySql.Append(" WHERE a.ListType = @listType");
                querySql.Append(" AND b.CompanyCode = @companyCode");

                if (!String.IsNullOrEmpty(userName))
                {
                    querySql.Append(" AND b.UserName = @userName");
                }

                querySql.Append(" ORDER BY b.UserName DESC");

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
                dt.Columns.Add("Id");
                dt.Columns.Add("UserName");
                dt.Columns.Add("DisplayName");
                dt.Columns.Add("Status");
                dt.Columns.Add("CreateTime");

                foreach (DbDataRecord rec in query)
                {
                    Code statusCode = codes.Where(m => m.Category == "STATUS" && m.CodeName == rec["Status"].ToString()).FirstOrDefault();

                    string createTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", rec["CreateTime"]);

                    DataRow dr = dt.NewRow();
                    dr["Id"] = rec["Id"].ToString();
                    dr["UserName"] = rec["UserName"].ToString();
                    dr["DisplayName"] = rec["DisplayName"].ToString();
                    dr["Status"] = statusCode == null ? "" : statusCode.Description;
                    dr["CreateTime"] = createTime;

                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        public MobileApp GetMobileAppByOwnerUriAndListType(string ownerUri, string listType)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileApp.Where(m => m.OwnerUri == ownerUri && m.ListType == listType).FirstOrDefault();
            }
        }

        public void Create(MobileApp model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                dataContext.AddToMobileApp(model);
                dataContext.SaveChanges();
            }
        }

        public MobileApp GetById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileApp.Where(m => m.Id == id).FirstOrDefault();
            }
        }

        public void Delete(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileApp entity = dataContext.MobileApp.Where(m => m.Id == id).FirstOrDefault();

                if (entity != null)
                {
                    dataContext.MobileApp.DeleteObject(entity);
                    dataContext.SaveChanges();
                }
            }
        }

        public void Update(MobileApp model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileApp entity = dataContext.MobileApp.Where(m => m.Id == model.Id).FirstOrDefault();

                entity.Etag = model.Etag;
                entity.Status = model.Status;
                entity.Updator = model.Updator;
                entity.UpdateTime = model.UpdateTime;

                dataContext.SaveChanges();
            }
        }

        public MobileApp GetMobileAppByOwnerUriAndListFileName(string ownerUri, string listFileName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileApp.Where(m => m.OwnerUri == ownerUri && m.ListFileName == listFileName).FirstOrDefault();
            }
        }
    }
}
