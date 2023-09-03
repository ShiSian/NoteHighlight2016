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
        public OneT() { }
        public OneT(string XmlString)
        {
            XElement element = XElement.Parse(XmlString);
            text = element.Value;
        }
        public OneT(XElement XmlElement)
        {
            text = XmlElement.Value;
        }


        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public override string GetStringData()
        {
            return "";
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
}
