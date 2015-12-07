using System;
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
    public class MobileUserFileService : IMobileUserFileService
    {
        public DataTable GetListByPage(string companyCode, string userName, int start, int limit, out int total)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                StringBuilder queryCountSql = new StringBuilder();
                queryCountSql.Append(@"SELECT a.Id FROM DataContext.MobileUserFile as a");
                queryCountSql.Append(" LEFT JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as b");
                queryCountSql.Append(" ON a.OwnerUri = b.OwnerUri");
                queryCountSql.Append(" WHERE a.FileType = '4'");
                queryCountSql.Append(" AND b.CompanyCode = @companyCode");
                
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
                querySql.Append(@"SELECT a.Id, b.UserName, b.DisplayName, a.FileName, a.FileSize, a.CreateTime FROM DataContext.MobileUserFile as a");
                querySql.Append(" LEFT JOIN OFTYPE(DataContext.MobileUser, MobileCms.Data.MobileUserExtension) as b");
                querySql.Append(" ON a.OwnerUri = b.OwnerUri");
                querySql.Append(" WHERE a.FileType = '4'");
                querySql.Append(" AND b.CompanyCode = @companyCode");
                
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

                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("UserName");
                dt.Columns.Add("DisplayName");
                dt.Columns.Add("FileName");
                dt.Columns.Add("FileSize");
                dt.Columns.Add("CreateTime");
                
                foreach (DbDataRecord rec in query)
                {
                    string createTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", rec["CreateTime"]);

                    DataRow dr = dt.NewRow();
                    dr["Id"] = rec["Id"].ToString();
                    dr["UserName"] = rec["UserName"] == null ? "" : rec["UserName"].ToString();
                    dr["DisplayName"] = rec["DisplayName"] == null ? "" : rec["DisplayName"].ToString();
                    dr["FileName"] = rec["FileName"] == null ? "" : rec["FileName"].ToString();
                    dr["FileSize"] = rec["FileSize"] == null ? "" : rec["FileSize"].ToString() + "M";
                    dr["CreateTime"] = createTime;
                    
                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        public MobileUserFile GetById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUserFile.Where(m => m.Id == id).FirstOrDefault();
            }
        }
    }
}