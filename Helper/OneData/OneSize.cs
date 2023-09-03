using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    class OneSize
    {
        private string width;
        private string height;

        public OneSize(XElement XElem)
        {
            width = XElem.Attribute("width").Value;
            height = XElem.Attribute("height").Value;
        }
    }
}
