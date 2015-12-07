using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;
using Newtonsoft.Json.Linq;

namespace MobileCms.Ajax
{
    /// <summary>
    /// Summary description for getmobileuserlocationintime
    /// </summary>
    public class getmobileuserlocationintime : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            JArray jsonArray = new JArray();

            WeiXinUserExtension weiXinUserExtension = new MobileCms.Common.BasePageWeiXinMember().GetWeiXinUserExtension();

            if (weiXinUserExtension != null)
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IWeiXinInMobileService weiXinInMobileService = container.Resolve<IWeiXinInMobileService>();

                List<WeiXinInMobile> weiXinInMobiles = weiXinInMobileService.GetWeiXinInMobileByWeiXinOwnerUri(weiXinUserExtension.OwnerUri);

                if (weiXinInMobiles != null && weiXinInMobiles.Count > 0)
                {
                    WeiXinInMobile weiXinInMobile = weiXinInMobiles.FirstOrDefault();

                    string mobileOwnerUri = weiXinInMobile.MobileOwnerUri;
                    string strStartTime = RequestHelp.GetString("startTime");
                    string strEndTime = RequestHelp.GetString("endTime");

                    DateTime startTime = Convert.ToDateTime(strStartTime);
                    DateTime endTime = Convert.ToDateTime(strEndTime);

                    IMobileUserLocationService mobileUserLocationService = container.Resolve<IMobileUserLocationService>();

                    List<MobileUserLocation> mobileUserLocations = mobileUserLocationService.GetMobileUserLocationByOwnerUriAndTime(mobileOwnerUri, startTime, endTime);

                    if (mobileUserLocations != null && mobileUserLocations.Count > 0)
                    {
                        for (int i = 0; i < mobileUserLocations.Count - 1; i++)
                        {
                            JObject jsonObject = new JObject();

                            string startLongitude = mobileUserLocations[i].Longitude;
                            string startLatitude = mobileUserLocations[i].Latitude;

                            string endLongitude = mobileUserLocations[i + 1].Longitude;
                            string endLatitude = mobileUserLocations[i + 1].Latitude;

                            jsonObject.Add("startLongitude", startLongitude);
                            jsonObject.Add("startLatitude", startLatitude);
                            jsonObject.Add("endLongitude", endLongitude);
                            jsonObject.Add("endLatitude", endLatitude);

                            jsonArray.Add(jsonObject);
                        }

                        context.Response.Write(jsonArray);
                        return;
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