using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    public class OneOE
    {
        // 属性部分
        private string author;
        private string authorInitials;
        private string authorResolutionID;
        private string lastModifiedBy;
        private string lastModifiedByInitials;
        private string lastModifiedByResolutionID;
        private string creationTime;
        private string lastModifiedTime;
        private string objectID;
        private string alignment;
        private string quickStyleIndex;
        private string spaceBefore;
        private string spaceAfter;
        private string spaceBetween;

        // 元素部分（灵活变化部分）
        public OneOE(XElement XElem)
        {

        }
    }

    class OneOEChildren
    {
        List<OneOE> oneOEs;

        public OneOEChildren(XElement XElem)
        {
            foreach (XElement item in XElem.Elements(OnePage.ns + "OE"))
            {
                OneOE TmpOneOE = new OneOE(item);
                oneOEs.Add(TmpOneOE);
            }
        }
    }
}
