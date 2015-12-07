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
    public class MobileAppItemService : IMobileAppItemService
    {
        public DataTable GetListByPage(string description, string listFileName, int start, int limit, out int total)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                StringBuilder queryCountSql = new StringBuilder();
                queryCountSql.Append(@"SELECT a.Id FROM DataContext.MobileAppItem as a");
                queryCountSql.Append(" JOIN DataContext.MobileAppConfig as b");
                queryCountSql.Append(" ON a.AppCodeName = b.CodeName");
                queryCountSql.Append(" JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as c");
                queryCountSql.Append(" ON a.OwnerUri = c.OwnerUri");
                queryCountSql.Append(" WHERE a.ListFileName = @listFileName");

                if (!String.IsNullOrEmpty(description))
                {
                    queryCountSql.Append(" AND b.Description like '%" + description + "%'");
                }

                ObjectQuery<DbDataRecord> queryCount = new ObjectQuery<DbDataRecord>(queryCountSql.ToString(), dataContext);
                queryCount.Parameters.Add(new ObjectParameter("listFileName", listFileName));
                
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
                querySql.Append(@"SELECT a.Id, a.AppCodeName, b.Description, b.PackageName, c.UserName, c.DisplayName FROM DataContext.MobileAppItem as a");
                querySql.Append(" JOIN DataContext.MobileAppConfig as b");
                querySql.Append(" ON a.AppCodeName = b.CodeName");
                querySql.Append(" JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as c");
                querySql.Append(" ON a.OwnerUri = c.OwnerUri");
                querySql.Append(" WHERE a.ListFileName = @listFileName");

                if (!String.IsNullOrEmpty(description))
                {
                    querySql.Append(" AND b.Description like '%" + description + "%'");
                }

                querySql.Append(" ORDER BY a.AppCodeName ASC");

                querySql.Append(@" SKIP @skip LIMIT @limit");

                ObjectQuery<DbDataRecord> query = new ObjectQuery<DbDataRecord>(querySql.ToString(), dataContext);
                query.Parameters.Add(new ObjectParameter("listFileName", listFileName));

                query.Parameters.Add(new ObjectParameter("skip", start));
                query.Parameters.Add(new ObjectParameter("limit", limit));

                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("UserName");
                dt.Columns.Add("DisplayName");
                dt.Columns.Add("AppCodeName");
                dt.Columns.Add("Description");
                dt.Columns.Add("PackageName");
                
                foreach (DbDataRecord rec in query)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = rec["Id"].ToString();
                    dr["UserName"] = rec["UserName"].ToString();
                    dr["DisplayName"] = rec["DisplayName"].ToString();
                    dr["AppCodeName"] = rec["AppCodeName"].ToString();
                    dr["Description"] = rec["Description"].ToString();
                    dr["PackageName"] = rec["PackageName"] == null ? "" : rec["PackageName"].ToString();
                    
                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        public void Create(MobileAppItem model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                dataContext.AddToMobileAppItem(model);
                dataContext.SaveChanges();
            }
        }

        public MobileAppItem GetById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileAppItem.Where(m => m.Id == id).FirstOrDefault();
            }
        }

        public void Delete(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileAppItem entity = dataContext.MobileAppItem.Where(m => m.Id == id).FirstOrDefault();

                if (entity != null)
                {
                    dataContext.MobileAppItem.DeleteObject(entity);
                    dataContext.SaveChanges();
                }
            }
        }

        public List<MobileAppItem> GetMobileAppItemByOwnerUriAndListFileName(string ownerUri, string listFileName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileAppItem.Where(m => m.OwnerUri == ownerUri && m.ListFileName == listFileName).ToList();
            }
        }

        public List<MobileAppItem> GetMobileAppItemsByAppCodeName(string appCodeName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileAppItem.Where(m => m.AppCodeName == appCodeName).ToList();
            }
        }
    }
}
