using System;
using System.Collections.Generic;
using System.Linq;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.WebsiteService
{
    public class MobileCardReaderReportService : IMobileCardReaderReportService
    {
        public List<MobileCardReaderReport> GetMobileCardReaderReportsByOwnerUriAndCreateTime(string ownerUri, DateTime startTime, DateTime endTime)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileCardReaderReport.Where(m => m.OwnerUri == ownerUri && m.CreateTime >= startTime && m.CreateTime <= endTime).OrderBy(m => m.CreateTime).ToList();
            }
        }
    }
}
