using System;
using System.Collections.Generic;
using System.Linq;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.WebsiteService
{
    public class MobileUserLocationService : IMobileUserLocationService
    {
        public List<MobileUserLocation> GetMobileUserLocations()
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUserLocation.Where(m => m.Status == "Y").OrderByDescending(m => m.CreateTime).ToList();
            }
        }

        public List<MobileUserLocation> GetMobileUserLocationByOwnerUriAndTime(string ownerUri, DateTime startTime, DateTime endTime)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUserLocation.Where(m => m.OwnerUri == ownerUri && m.CreateTime >= startTime && m.CreateTime <= endTime).OrderBy(m => m.CreateTime).ToList();
            }
        }
    }
}
