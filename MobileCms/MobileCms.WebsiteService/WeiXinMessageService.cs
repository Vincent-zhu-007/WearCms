using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms.WebsiteService
{
    public class WeiXinMessageService : IWeiXinMessageService
    {
        public void Create(WeiXinMessage model)
        {
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            using (DataContext dataContext = container.ResolveObjectContext<DataContext>())
            {
                dataContext.AddToWeiXinMessage(model);
                dataContext.SaveChanges();
            }
        }
    }
}
