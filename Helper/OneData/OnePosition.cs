using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    class OnePosition
    {
        private string x;
        private string y;
        private string z;

        public OnePosition(XElement XElem)
        {
            x = XElem.Attribute("x").Value;
            y = XElem.Attribute("y").Value;
            z = XElem.Attribute("z").Value;
        }
    }
}
