using System.Collections.Generic;
using System.Linq;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.WebsiteService
{
    public class MobileUserExtensionService : IMobileUserExtensionService
    {
        public MobileUserExtension GetMobileUserExtensionById(string id)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileUserExtension model = dataContext.MobileUser.OfType<MobileUserExtension>().Where(m => m.Id == id).FirstOrDefault();

                return model;
            }
        }

        public MobileUserExtension GetMobileUserExtensionByUserName(string userName)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileUserExtension model = dataContext.MobileUser.OfType<MobileUserExtension>().Where(m => m.UserName == userName).FirstOrDefault();

                return model;
            }
        }

        public MobileUserExtension GetMobileUserExtensionByOwnerUri(string ownerUri)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileUserExtension model = dataContext.MobileUser.OfType<MobileUserExtension>().Where(m => m.OwnerUri == ownerUri && m.Status == "Y").FirstOrDefault();

                return model;
            }
        }

        public void Update(MobileUserExtension model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileUserExtension entity = dataContext.MobileUser.OfType<MobileUserExtension>().Where(m => m.Id == model.Id).FirstOrDefault();

                entity.DisplayName = model.DisplayName;
                entity.MobilePhone = model.MobilePhone;
                entity.Gender = model.Gender == null ? "" : model.Gender;
                entity.Birthday = model.Birthday == null ? "" : model.Birthday;
                entity.Status = model.Status;
                entity.Updator = model.Updator;
                entity.UpdateTime = model.UpdateTime;

                dataContext.SaveChanges();
            }
        }

        public List<MobileUserExtension> GetMobileUserExtensionByCompanyCode(string companyCode)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                return dataContext.MobileUser.OfType<MobileUserExtension>().Where(m => m.CompanyCode == companyCode).OrderBy(m => m.MobilePhone).ToList();
            }
        }

        public MobileUserExtension GetMobileUserExtensionByOwnerUriUnStatus(string ownerUri)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                MobileUserExtension model = dataContext.MobileUser.OfType<MobileUserExtension>().Where(m => m.OwnerUri == ownerUri).FirstOrDefault();

                return model;
            }
        }
    }
}
