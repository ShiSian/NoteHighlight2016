using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper
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
            if (XmlElement.Attribute("style") != null)
            {
                style = XmlElement.Attribute("style").Value;
            }
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
            if (XElem != null)
            {
                if (XElem.Attribute("numberSequence") != null)
                {
                    numberSequence = XElem.Attribute("numberSequence").Value;

                }
                numberFormat = XElem.Attribute("numberFormat").Value;
                fontSize = XElem.Attribute("fontSize").Value;
                if (XElem.Attribute("font") != null)
                {
                    font = XElem.Attribute("font").Value;

                }
                if (XElem.Attribute("bold") != null)
                {
                    bold = XElem.Attribute("bold").Value;
                }
                language = XElem.Attribute("language").Value;
                text = XElem.Attribute("text").Value;
            }
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
        private List<OneItem> oneItems = new List<OneItem>();

        public OneOE(XElement XElem)
        {
            if (XElem.Attribute("author") != null)
            {
                author = XElem.Attribute("author").Value;
            }
            if (XElem.Attribute("authorInitials") != null)
            {
                authorInitials = XElem.Attribute("authorInitials").Value;

            }
            if (XElem.Attribute("authorResolutionID") != null)
            {
                authorResolutionID = XElem.Attribute("authorResolutionID").Value;
            }
            if (XElem.Attribute("lastModifiedBy") != null)
            {
                lastModifiedBy = XElem.Attribute("lastModifiedBy").Value;
            }
            if (XElem.Attribute("lastModifiedByInitials") != null)
            {
                lastModifiedByInitials = XElem.Attribute("lastModifiedByInitials").Value;
            }
            lastModifiedByResolutionID = XElem.Attribute("lastModifiedByResolutionID").Value;
            creationTime = XElem.Attribute("creationTime").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            objectID = XElem.Attribute("objectID").Value;
            alignment = XElem.Attribute("alignment").Value;
            if (XElem.Attribute("quickStyleIndex") != null)
            {
                quickStyleIndex = XElem.Attribute("quickStyleIndex").Value;
            }
            if (XElem.Attribute("spaceBefore") != null)
            {
                spaceBefore = XElem.Attribute("spaceBefore").Value;
            }
            if (XElem.Attribute("spaceAfter") != null)
            {
                spaceAfter = XElem.Attribute("spaceAfter").Value;
            }
            if (XElem.Attribute("spaceBetween") != null)
            {
                spaceBetween = XElem.Attribute("spaceBetween").Value;
            }


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
        List<OneOE> oneOEs = new List<OneOE>();

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
            callbackID = XElem.Attribute("callbackID").Value;
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
            if (XElem.Attribute("z") != null)
            {
                z = XElem.Attribute("z").Value;

            }
            width = XElem.Attribute("width").Value;
            height = XElem.Attribute("height").Value;
        }
    }

    public class OneOCRData
    {
        private string lang;

        private OneOCRText oneOCRText;
        private List<OneOCRToken> oneOCRTokens = new List<OneOCRToken>();

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
