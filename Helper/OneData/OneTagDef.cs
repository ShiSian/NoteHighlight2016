using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    class OneTagDef
    {
        private string index;
        private string type;
        private string symbol;
        private string fontColor;
        private string highlightColor;
        private string name;

        public OneTagDef(XElement XElem)
        {
            index = XElem.Attribute("index").Value;
            type = XElem.Attribute("type").Value;
            symbol = XElem.Attribute("symbol").Value;
            fontColor = XElem.Attribute("fontColor").Value;
            highlightColor = XElem.Attribute("highlightColor").Value;
            name = XElem.Attribute("name").Value;
        }
    }
}
