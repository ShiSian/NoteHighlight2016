using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper
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

    class OneSize
    {
        private string width;
        private string height;
        private string isSetByUser;

        public OneSize(XElement XElem)
        {
            width = XElem.Attribute("width").Value;
            height = XElem.Attribute("height").Value;
            if (XElem.Attribute("isSetByUser") != null)
            {
                isSetByUser = XElem.Attribute("isSetByUser").Value;
            }
        }
    }

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

            onePosition = new OnePosition(XElem.Element(OneDataHelper.OneSpace + "Position"));
            oneSize = new OneSize(XElem.Element(OneDataHelper.OneSpace + "Size"));
            oneOEChildren = new OneOEChildren(XElem.Element(OneDataHelper.OneSpace + "OEChildren"));
        }

        public string ToCSV()
        {
            return oneOEChildren.ToCSV();
        }
    }
}
