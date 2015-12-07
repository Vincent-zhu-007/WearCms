using System;

namespace MobileCms.Data
{
    [Serializable]
    public class XmlRpcParamenter
    {
        public XmlRpcParamenter() { }

        private string paraName;

        public string ParaName
        {
            get { return paraName; }
            set { paraName = value; }
        }
        private string paraType;

        public string ParaType
        {
            get { return paraType; }
            set { paraType = value; }
        }
        private string paraValue;

        public string ParaValue
        {
            get { return paraValue; }
            set { paraValue = value; }
        }
        private int paraSort;

        public int ParaSort
        {
            get { return paraSort; }
            set { paraSort = value; }
        }
    }
}
