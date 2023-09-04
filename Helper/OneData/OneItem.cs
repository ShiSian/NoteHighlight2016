using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    public abstract class OneItem
    {
        abstract public string GetStringData();
    }

    public class OneT : OneItem
    {
        private string style;
        private string value;

        public OneT(XElement XmlElement)
        {
            style = XmlElement.Attribute("style").Value;
            value = XmlElement.Value;
        }

        public override string GetStringData()
        {
            return value;
        }
    }

    public class OneNumber : OneItem
    {
        private string numberSequence;
        private string numberFormat;
        private string fontSize;
        private string font;
        private string bold;
        private string language;
        private string text;

        public OneNumber(XElement XElem)
        {
            numberSequence = XElem.Attribute("numberSequence").Value;
            numberFormat = XElem.Attribute("numberFormat").Value;
            fontSize = XElem.Attribute("fontSize").Value;
            font = XElem.Attribute("font").Value;
            bold = XElem.Attribute("bold").Value;
            language = XElem.Attribute("language").Value;
            text = XElem.Attribute("text").Value;
        }

        public override string GetStringData()
        {
            return text;
        }
    }

    public class OneList : OneItem
    {
        private OneNumber oneNumber;

        public OneList(XElement XElem)
        {
            oneNumber = new OneNumber(XElem.Element(OnePage.ns + "Number"));
        }

        public override string GetStringData()
        {
            return oneNumber.GetStringData();
        }
    }

    public class OneOE : OneItem
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

        List<OneItem> oneItems;

        // 元素部分（灵活变化部分）
        public OneOE(XElement XElem)
        {
            foreach (XElement item in XElem.Elements())
            {
                if (item.Name == (OnePage.ns + "T"))
                {
                    OneT TmpOneT = new OneT(item);
                    oneItems.Add(TmpOneT);
                }
                else if (item.Name == (OnePage.ns + "List"))
                {
                    OneList TmpOneList = new OneList(item);

                }
            }
        }

        public override string GetStringData()
        {
            throw new NotImplementedException();
        }
    }

    class OneOEChildren : OneItem
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

        public override string GetStringData()
        {
            throw new NotImplementedException();
        }
    }
}
