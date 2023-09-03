using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
 

    class OnePageSettings
    {
        private string RTL;
        private string color;
        private OnePageSize pageSize;

        public OnePageSettings() { }
        public OnePageSettings(XElement XElem)
        {
            RTL = XElem.Attribute("RTL").Value;
            color = XElem.Attribute("color").Value;            
            pageSize = new OnePageSize(XElem.Element(OnePage.ns + "PageSize"));
        }

    }
}
