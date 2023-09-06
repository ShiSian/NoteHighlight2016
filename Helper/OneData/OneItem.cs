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
            oneNumber = new OneNumber(XElem.Element(OneDataHelper.OneSpace + "Number"));
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

        // 元素部分（灵活变化部分）
        private List<OneItem> oneItems;

        public OneOE(XElement XElem)
        {
            author = XElem.Attribute(OneDataHelper.OneSpace + "author").Value;
            authorInitials = XElem.Attribute(OneDataHelper.OneSpace + "authorInitials").Value;
            authorResolutionID = XElem.Attribute(OneDataHelper.OneSpace + "authorResolutionID").Value;
            lastModifiedBy = XElem.Attribute(OneDataHelper.OneSpace + "lastModifiedBy").Value;
            lastModifiedByInitials = XElem.Attribute(OneDataHelper.OneSpace + "lastModifiedByInitials").Value;
            lastModifiedByResolutionID = XElem.Attribute(OneDataHelper.OneSpace + "lastModifiedByResolutionID").Value;
            creationTime = XElem.Attribute(OneDataHelper.OneSpace + "creationTime").Value;
            lastModifiedTime = XElem.Attribute(OneDataHelper.OneSpace + "lastModifiedTime").Value;
            objectID = XElem.Attribute(OneDataHelper.OneSpace + "objectID").Value;
            alignment = XElem.Attribute(OneDataHelper.OneSpace + "alignment").Value;
            quickStyleIndex = XElem.Attribute(OneDataHelper.OneSpace + "quickStyleIndex").Value;
            spaceBefore = XElem.Attribute(OneDataHelper.OneSpace + "spaceBefore").Value;
            spaceAfter = XElem.Attribute(OneDataHelper.OneSpace + "spaceAfter").Value;
            spaceBetween = XElem.Attribute(OneDataHelper.OneSpace + "spaceBetween").Value;


            foreach (XElement item in XElem.Elements())
            {
                OneDataHelper.AutoConstructItem(ref oneItems, item);
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
            foreach (XElement item in XElem.Elements(OneDataHelper.OneSpace + "OE"))
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

    public class OneCallbackID
    {
        private string callbackID;

        public OneCallbackID(XElement XElem)
        {
            callbackID = XElem.Attribute(OneDataHelper.OneSpace + "CallbackID").Value;
        }
    }

    public class OneOCRText
    {
        private string Value;

        public OneOCRText(XElement XElem)
        {
            Value = XElem.Value;
        }
    }

    public class OneOCRToken
    {
        private string startPos;
        private string region;
        private string line;
        private string x;
        private string y;
        private string z;
        private string width;
        private string height;

        public OneOCRToken(XElement XElem)
        {
            startPos = XElem.Attribute("startPos").Value;
            region = XElem.Attribute("region").Value;
            line = XElem.Attribute("line").Value;
            x = XElem.Attribute("x").Value;
            y = XElem.Attribute("y").Value;
            z = XElem.Attribute("z").Value;
            width = XElem.Attribute("width").Value;
            height = XElem.Attribute("height").Value;
        }
    }

    public class OneOCRData
    {
        private string lang;

        private OneOCRText oneOCRText;
        private List<OneOCRToken> oneOCRTokens;

        public OneOCRData(XElement XElem)
        {
            lang = XElem.Attribute("lang").Value;

            oneOCRText = new OneOCRText(XElem.Element(OneDataHelper.OneSpace + "OCRText"));
            foreach (XElement item in XElem.Elements(OneDataHelper.OneSpace + "OCRToken"))
            {
                oneOCRTokens.Add(new OneOCRToken(item));
            }
        }
    }

    public class OneImage : OneItem
    {
        private OneSize oneSize;
        private OneCallbackID oneCallbackID;
        private OneOCRData oneOCRData;

        public OneImage(XElement XElem)
        {
            oneSize = new OneSize(XElem.Element(OneDataHelper.OneSpace + "Size"));
            oneCallbackID = new OneCallbackID(XElem.Element(OneDataHelper.OneSpace + "CallbackID"));
            oneOCRData = new OneOCRData(XElem.Element(OneDataHelper.OneSpace + "OCRData"));
        }


        public override string GetStringData()
        {
            throw new NotImplementedException();
        }
    }
}
