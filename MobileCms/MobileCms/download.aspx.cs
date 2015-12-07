using System;
using System.IO;
using System.Net;
using MobileCms.Common;
using MobileCms.Container;
using MobileCms.Data;
using MobileCms.Website;

namespace MobileCms
{
    public partial class download : BasePageMember
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = RequestHelp.GetString("id");

            if (!String.IsNullOrEmpty(id))
            {
                IComponentContainer container = ComponentContainerFactory.CreateContainer();
                IMobileUserFileService mobileUserFileService = container.Resolve<IMobileUserFileService>();

                MobileUserFile mobileUserFile = mobileUserFileService.GetById(id);

                if (mobileUserFile != null)
                {
                    string fileType = mobileUserFile.FileType;
                    string fileUrl = mobileUserFile.FileUrl;

                    if (!String.IsNullOrEmpty(fileUrl) && !String.IsNullOrEmpty(fileType))
                    {
                        string baseUrl = ConfigHelp.GetConfigString("MobileServerHost");
                        string port = ConfigHelp.GetConfigString("MobileServerPort");

                        fileUrl = mobileUserFile.FileUrl == null ? "" : baseUrl + ":" + port + mobileUserFile.FileUrl;

                        string fileName = "";
                        int indexFileName = fileUrl.LastIndexOf('/') + 1;
                        fileName = fileUrl.Substring(indexFileName, fileUrl.Length - indexFileName);

                        int indexSuffix = fileName.LastIndexOf('.') + 1;
                        string suffix = fileName.Substring(indexSuffix, fileName.Length - indexSuffix);

                        string downloadSuffix = ConfigHelp.GetConfigString("DownloadSuffix");
                        string[] array = downloadSuffix.Split(',');

                        bool isAllow = false;
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (suffix.Equals(array[i]))
                            {
                                isAllow = true;
                                break;
                            }
                        }

                        if (isAllow)
                        {
                            DownUrltoFile(fileUrl, fileName);
                        }
                        else
                        {
                            Response.Write("下载失败，" + suffix + "格式文件不允许下载");
                            return;
                        }
                    }
                }
            }
        }

        protected void DownUrltoFile(string url, string fileName)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Timeout = 10000;
                this.NotFolderIsCreate(fileName);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream readStream = httpWebResponse.GetResponseStream();
                BinaryReader SplitFileReader = new BinaryReader(readStream);

                Response.ContentType = "application/save";
                Response.HeaderEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));

                byte[] bytes = SplitFileReader.ReadBytes(Convert.ToInt32(httpWebResponse.ContentLength));
                Response.BinaryWrite(bytes);
                SplitFileReader.Close();
                readStream.Close();
            }
            catch (Exception ex)
            {
                Response.Write("下载失败，失败原因：" + ex.Message);
                return;
            }
        }

        protected void NotFolderIsCreate(string filename)
        {
            string fileAtDir = Server.MapPath(Path.GetDirectoryName(filename));
            if (!Directory.Exists(fileAtDir))
                Directory.CreateDirectory(fileAtDir);
        }
    }
}