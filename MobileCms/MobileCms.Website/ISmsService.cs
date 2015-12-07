using MobileCms.Data;

namespace MobileCms.Website
{
    public interface ISmsService
    {
        Sms GetSmsByMobile(string mobile);

        Sms GetSmsByMobileAndOptionCategory(string mobile, string optionCategory);

        void Create(Sms model);

        void Update(Sms model);
    }
}
