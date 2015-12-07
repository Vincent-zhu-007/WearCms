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
    public class MobileContactMemberService : IMobileContactMemberService
    {
        public DataTable GetListByPage(string companyCode, string displayName, string listFileName, string ownerUri, int start, int limit, out int total)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            ICodeService codeService = container.Resolve<ICodeService>();

            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                StringBuilder queryCountSql = new StringBuilder();
                queryCountSql.Append(@"SELECT a.Id FROM DataContext.MobileContactMember as a");
                queryCountSql.Append(" WHERE a.ListFileName = @listFileName");
                queryCountSql.Append(" AND a.OwnerUri = @ownerUri");

                if (!String.IsNullOrEmpty(displayName))
                {
                    queryCountSql.Append(" AND a.DisplayName = @displayName");
                }

                ObjectQuery<DbDataRecord> queryCount = new ObjectQuery<DbDataRecord>(queryCountSql.ToString(), dataContext);
                queryCount.Parameters.Add(new ObjectParameter("listFileName", listFileName));
                queryCount.Parameters.Add(new ObjectParameter("ownerUri", ownerUri));

                if (!String.IsNullOrEmpty(displayName))
                {
                    queryCount.Parameters.Add(new ObjectParameter("displayName", displayName));
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
                querySql.Append(@"SELECT a.Id, a.OwnerUri, a.DisplayName, a.MobilePhone, a.NumButton FROM DataContext.MobileContactMember as a");
                querySql.Append(" WHERE a.ListFileName = @listFileName");
                querySql.Append(" AND a.OwnerUri = @ownerUri");

                if (!String.IsNullOrEmpty(displayName))
                {
                    querySql.Append(" AND a.DisplayName = @displayName");
                }

                querySql.Append(" ORDER BY a.NumButton ASC");

                querySql.Append(@" SKIP @skip LIMIT @limit");

                ObjectQuery<DbDataRecord> query = new ObjectQuery<DbDataRecord>(querySql.ToString(), dataContext);
                query.Parameters.Add(new ObjectParameter("listFileName", listFileName));
                query.Parameters.Add(new ObjectParameter("ownerUri", ownerUri));

                if (!String.IsNullOrEmpty(displayName))
                {
                    query.Parameters.Add(new ObjectParameter("displayName", displayName));
                }

                query.Parameters.Add(new ObjectParameter("skip", start));
                query.Parameters.Add(new ObjectParameter("limit", limit));

                List<Code> codes = codeService.GetCodeCache(companyCode);

                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("OwnerUri");
                dt.Columns.Add("DisplayName");
                dt.Columns.Add("MobilePhone");
                dt.Columns.Add("NumButton");
                
                foreach (DbDataRecord rec in query)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = rec["Id"].ToString();
                    dr["OwnerUri"] = rec["OwnerUri"].ToString();
                    dr["DisplayName"] = rec["DisplayName"] == null ? "" : rec["DisplayName"].ToString();
                    dr["MobilePhone"] = rec["MobilePhone"] == null ? "" : rec["MobilePhone"].ToString();
                    dr["NumButton"] = rec["NumButton"] == null ? "" : rec["NumButton"].ToString();
                    
                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        public List<MobileContactMember> GetMobileContactMembersByOwnerUriAndListFileName(string ownerUri, string listFileName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileContactMember.Where(m => m.OwnerUri == ownerUri && m.ListFileName == listFileName).OrderBy(m => m.NumButton).ToList();
            }
        }

        public void Create(MobileContactMember model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                dataContext.AddToMobileContactMember(model);
                dataContext.SaveChanges();
            }
        }

        public MobileContactMember GetById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileContactMember.Where(m => m.Id == id).FirstOrDefault();
            }
        }

        public void Delete(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileContactMember entity = dataContext.MobileContactMember.Where(m => m.Id == id).FirstOrDefault();

                if (entity != null)
                {
                    dataContext.MobileContactMember.DeleteObject(entity);
                    dataContext.SaveChanges();
                }
            }
        }

        public void Update(MobileContactMember model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileContactMember entity = dataContext.MobileContactMember.Where(m => m.Id == model.Id).FirstOrDefault();

                entity.DisplayName = model.DisplayName;
                entity.MobilePhone = model.MobilePhone;
                entity.NumButton = model.NumButton;
                
                dataContext.SaveChanges();
            }
        }
    }
}
