using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper
{
    public class OneTitle
    {
        // 属性部分
        private string lang;

        // 元素部分
        private OneOE oneOE;

        // 构造函数
        public OneTitle(XElement XElem)
        {
            lang = XElem.Attribute("lang").Value;
            oneOE = new OneOE(XElem.Element(OneDataHelper.OneSpace + "OE"));
        }

    }
}
