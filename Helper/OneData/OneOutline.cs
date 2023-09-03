using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    class OneOutline
    {
        // 属性
        private string author;
        private string authorInitials;
        private string lastModifiedBy;
        private string lastModifiedByInitials;
        private string lastModifiedTime;
        private string objectID;

        // 元素
        OnePosition onePosition;
        OneSize oneSize;
        OneOEChildren oneOEChildren;

        public OneOutline(XElement XElem)
        {
            author = XElem.Attribute("author").Value;
            authorInitials = XElem.Attribute("authorInitials").Value;
            lastModifiedBy = XElem.Attribute("lastModifiedBy").Value;
            lastModifiedByInitials = XElem.Attribute("lastModifiedByInitials").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            objectID = XElem.Attribute("objectID").Value;

            onePosition = new OnePosition(XElem.Element(OnePage.ns + "Position"));
            oneSize = new OneSize(XElem.Element(OnePage.ns + "Size"));
            oneOEChildren = new OneOEChildren(XElem.Element(OnePage.ns + "OEChildren"));
        }
    }
}
