using System;
using System.Collections.Generic;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileCardReaderReportService
    {
        List<MobileCardReaderReport> GetMobileCardReaderReportsByOwnerUriAndCreateTime(string ownerUri, DateTime startTime, DateTime endTime);
    }
}
