using System;
using System.Collections.Generic;
using System.Linq;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms
{
    public partial class daysummary : BasePageMember
    {
        public string displayName = "";
        public string withinData = "";
        public string outsideData = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string ownerUri = RequestHelp.GetString("ownerUri");

            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IMobileUserExtensionService mobileUserExtensionService = container.Resolve<IMobileUserExtensionService>();

            MobileUserExtension mobileUserExtension = mobileUserExtensionService.GetMobileUserExtensionByOwnerUriUnStatus(ownerUri);

            displayName = mobileUserExtension.DisplayName;
            
            //string strNow = "2015-07-26 17:42:50";
            //DateTime now = DateTime.Parse(strNow);

            DateTime now = DateTime.Now;
            string year = now.Year.ToString();
            string month = now.Month.ToString();
            string day = now.Day.ToString();
            string startHour = "00";
            string endHour = "23";
            string startMinute = "00";
            string endMinute = "59";
            string startSecond = "00";
            string endSecond = "59";

            string strStartTime = year + "-" + month + "-" + day + " " + startHour + ":" + startMinute + ":" + startSecond;
            string strEndTime = year + "-" + month + "-" + day + " " + endHour + ":" + endMinute + ":" + endSecond;

            DateTime startTime = DateTime.Parse(strStartTime);
            DateTime endTime = DateTime.Parse(strEndTime);

            InitChartData(ownerUri, startTime, endTime);
        }

        protected void butSearch_Click(object sender, EventArgs e)
        {
            MobileUser mobileUser = GetMobileUser();

            string ownerUri = RequestHelp.GetString("ownerUri");

            string searchDate = this.searchDate.Text;

            string strStartTime = searchDate + " " + "00:00:00";
            string strEndTime = searchDate + " " + "23:59:59";

            DateTime startTime = DateTime.Parse(strStartTime);
            DateTime endTime = DateTime.Parse(strEndTime);

            InitChartData(ownerUri, startTime, endTime);
        }

        private void InitChartData(string ownerUri, DateTime startTime, DateTime endTime)
        {
            withinData = "";
            outsideData = "";

            string withinEquipmentNo = ConfigHelp.GetConfigString("WithinEquipmentNo");
            string outsideEquipmentNo = ConfigHelp.GetConfigString("OutsideEquipmentNo");

            DateTime tempStartTime = startTime.AddDays(-1);
            
            IComponentContainer container = ComponentContainerFactory.CreateContainer();
            IMobileCardReaderReportService mobileCardReaderReportService = container.Resolve<IMobileCardReaderReportService>();

            List<MobileCardReaderReport> mobileCardReaderReports = mobileCardReaderReportService.GetMobileCardReaderReportsByOwnerUriAndCreateTime(ownerUri, tempStartTime, endTime);

            if (mobileCardReaderReports != null && mobileCardReaderReports.Count > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    int within = 0;
                    int outside = 0;

                    List<MobileCardReaderReport> listForHour = mobileCardReaderReports.Where(m => m.Day == startTime.Day.ToString() && m.Hour == i.ToString()).ToList();

                    if (listForHour != null && listForHour.Count > 0)
                    {
                        if (listForHour.Count == 1)
                        {
                            MobileCardReaderReport tempMobileCardReaderReport = listForHour.FirstOrDefault();
                            DateTime tempCreateTime = DateTime.Parse(tempMobileCardReaderReport.CreateTime.ToString());

                            if (tempMobileCardReaderReport.EquipmentNo.Equals(withinEquipmentNo))
                            {
                                //within
                                within = 60 - tempCreateTime.Minute;
                                outside = tempCreateTime.Minute;
                            }
                            else if (tempMobileCardReaderReport.EquipmentNo.Equals(outsideEquipmentNo))
                            {
                                //outside
                                outside = 60 - tempCreateTime.Minute;
                                within = tempCreateTime.Minute;
                            }
                        }
                        else
                        {
                            MobileCardReaderReport firstMobileCardReaderReport = listForHour.FirstOrDefault();
                            DateTime fristCreateTime = DateTime.Parse(firstMobileCardReaderReport.CreateTime.ToString());

                            bool isIn = false;

                            MobileCardReaderReport tempMobileCardReaderReport = GetLastMobileCardReaderReport(mobileCardReaderReports, fristCreateTime, fristCreateTime.Hour);

                            if (tempMobileCardReaderReport != null)
                            {
                                if (tempMobileCardReaderReport.EquipmentNo.Equals(withinEquipmentNo))
                                {
                                    isIn = true;
                                }
                            }

                            foreach (MobileCardReaderReport m in listForHour)
                            {
                                DateTime createTime = DateTime.Parse(m.CreateTime.ToString());

                                if (m.EquipmentNo.Equals(withinEquipmentNo))
                                {
                                    if (isIn)
                                    {
                                        //within = within + createTime.Minute;
                                        isIn = true;
                                    }
                                    else
                                    {
                                        outside = outside + (createTime.Minute - within);
                                        isIn = true;
                                    }
                                }
                                else if (m.EquipmentNo.Equals(outsideEquipmentNo))
                                {
                                    if (isIn)
                                    {
                                        within = within + createTime.Minute;
                                        isIn = false;
                                    }
                                    else
                                    {
                                        //outside = outside + createTime.Minute;
                                        isIn = false;
                                    }
                                }
                            }

                            MobileCardReaderReport lastMobileCardReaderReport = listForHour.OrderByDescending(m => m.CreateTime).FirstOrDefault();
                            DateTime lastCreateTime = DateTime.Parse(lastMobileCardReaderReport.CreateTime.ToString());

                            if (lastMobileCardReaderReport.EquipmentNo.Equals(withinEquipmentNo))
                            {
                                within = within + (60 - lastCreateTime.Minute);
                            }
                            else if (lastMobileCardReaderReport.EquipmentNo.Equals(outsideEquipmentNo))
                            {
                                outside = outside + (60 - lastCreateTime.Minute);
                            }
                        }
                    }
                    else
                    {
                        MobileCardReaderReport tempMobileCardReaderReport = GetLastMobileCardReaderReport(mobileCardReaderReports, startTime, i);

                        if (tempMobileCardReaderReport != null)
                        {
                            if (tempMobileCardReaderReport.EquipmentNo.Equals(withinEquipmentNo))
                            {
                                //within
                                within = 60;
                            }
                            else if (tempMobileCardReaderReport.EquipmentNo.Equals(outsideEquipmentNo))
                            {
                                //outside
                                outside = 60;
                            }
                        }
                        else
                        {
                            //outside
                            outside = 60;
                        }
                    }

                    withinData = withinData + within.ToString() + ",";
                    outsideData = outsideData + outside.ToString() + ",";
                }

                withinData = withinData.TrimEnd(',');
                outsideData = outsideData.TrimEnd(',');

                withinData = "[" + withinData + "]";
                outsideData = "[" + outsideData + "]";
            }
            else
            {
                withinData = "[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]";
                outsideData = "[60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,]";
            }
        }

        private MobileCardReaderReport GetLastMobileCardReaderReport(List<MobileCardReaderReport> mobileCardReaderReports, DateTime currentDateTime, int currentHour)
        {
            string year = currentDateTime.Year.ToString();
            string month = currentDateTime.Month.ToString();
            string day = currentDateTime.Day.ToString();

            string hour = "";
            if (currentHour == 0)
            {
                hour = "00";
            }
            else
            {
                if (currentHour < 10)
                {
                    hour = "0" + currentHour.ToString();
                }
                else
                {
                    hour = currentHour.ToString();
                }
            }

            string minute = "59";
            string second = "59";

            string strTempDate = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;

            DateTime tempDate = DateTime.Parse(strTempDate);

            MobileCardReaderReport tempMobileCardReaderReport = mobileCardReaderReports.Where(m => m.CreateTime <= tempDate).OrderByDescending(m => m.CreateTime).FirstOrDefault();

            return tempMobileCardReaderReport;
        }
    }
}