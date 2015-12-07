using System.Collections.Generic;
using MobileCms.Data;

namespace MobileCms.Website
{
    public interface IRpcService
    {
        void SendRpcCommand(string companyCode, string methodName, List<XmlRpcParamenter> xmlRpcParamenters);

        void runCmdUpdateApp(string ownerUri, string companyCode);

        void runCmdUpdateContact(string ownerUri, string companyCode);
    }
}
