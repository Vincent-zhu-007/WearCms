using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for getmobileuserlocation
    /// </summary>
    public class getmobileuserlocation : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JArray jsonArray = new JArray();

            MobileUser mobileUser = new MobileCms.Common.BasePageMember().GetMobileUser();

            if (mobileUser != null)
            {
                string companyCode = mobileUser.CompanyCode;
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileUserService mobileUserService = container.Resolve<IMobileUserService>();

                List<MobileUser> mobileUsers = mobileUserService.GetMobileUserByCompanyCode(companyCode);

                if (mobileUsers != null && mobileUsers.Count > 0)
                {
                    IMobileUserLocationService mobileUserLocationService = container.Resolve<IMobileUserLocationService>();
                    List<MobileUserLocation> mobileUserLocations = mobileUserLocationService.GetMobileUserLocations();

                    if (mobileUserLocations != null && mobileUserLocations.Count > 0)
                    {
                        foreach (MobileUser tempMobileUser in mobileUsers)
                        {
                            MobileUserLocation mobileUserLocation = mobileUserLocations.Where(m => m.OwnerUri == tempMobileUser.OwnerUri).FirstOrDefault();

                            if (mobileUserLocation != null)
                            {
                                JObject jsonObject = new JObject();
                                jsonObject.Add("ownerUri", mobileUserLocation.OwnerUri);
                                jsonObject.Add("displayName", tempMobileUser.DisplayName);
                                jsonObject.Add("createTime", mobileUserLocation.CreateTime.ToString());
                                jsonObject.Add("longitude", mobileUserLocation.Longitude);
                                jsonObject.Add("latitude", mobileUserLocation.Latitude);
                                jsonObject.Add("address", mobileUserLocation.Address == null ? "" : mobileUserLocation.Address);

                                jsonArray.Add(jsonObject);
                            }
                        }
                    }
                }
            }

            context.Response.Write(jsonArray);
            return;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}