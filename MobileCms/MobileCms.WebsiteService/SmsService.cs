using System.Linq;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.WebsiteService
{
    public class SmsService : ISmsService
    {
        public Sms GetSmsByMobile(string mobile)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                Sms model = dataContext.Sms.Where(m => m.Mobile == mobile).FirstOrDefault();

                return model;
            }
        }

        public Sms GetSmsByMobileAndOptionCategory(string mobile, string optionCategory)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                Sms model = dataContext.Sms.Where(m => m.Mobile == mobile && m.OptionCategory == optionCategory).FirstOrDefault();

                return model;
            }
        }

        public void Create(Sms model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                dataContext.AddToSms(model);
                dataContext.SaveChanges();
            }
        }

        public void Update(Sms model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                Sms sms = dataContext.Sms.Where(m => m.Id == model.Id).FirstOrDefault();

                sms.Mobile = model.Mobile;
                sms.OptionCategory = model.OptionCategory;
                sms.SmsCode = model.SmsCode;
                sms.SendTime = model.SendTime;
                sms.CreateTime = model.CreateTime;

                dataContext.SaveChanges();
            }
        }
    }
}
