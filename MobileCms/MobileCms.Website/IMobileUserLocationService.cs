using System;
using System.Collections.Generic;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IMobileUserLocationService
    {
        List<MobileUserLocation> GetMobileUserLocations();

        List<MobileUserLocation> GetMobileUserLocationByOwnerUriAndTime(string ownerUri, DateTime startTime, DateTime endTime);
    }
}
