using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    public class OneImage : OneItem
    {
        private OneSize oneSize;
        private OneCallbackID oneCallbackID;
        private OneOCRData oneOCRData;

        public OneImage(XElement XElem, int InIndentNum)
        {
            IndentNum = InIndentNum;

            if (XElem.Element(OneDataHelper.OneSpace + "Size") != null)
            {
                oneSize = new OneSize(XElem.Element(OneDataHelper.OneSpace + "Size"));
            }
            if (XElem.Element(OneDataHelper.OneSpace + "CallbackID") != null)
            {
                oneCallbackID = new OneCallbackID(XElem.Element(OneDataHelper.OneSpace + "CallbackID"));
            }
            if (XElem.Element(OneDataHelper.OneSpace + "OCRData") != null)
            {
                oneOCRData = new OneOCRData(XElem.Element(OneDataHelper.OneSpace + "OCRData"));
            }
        }

        public string GetImageBase64()
        {
            return OneDataHelper.GetImageBase64(oneCallbackID.GetCallbackID());
        }


        public override string ToStr()
        {
            return "OneImage";
        }

        public override string ToCSV()
        {
            return "OneIamge";
        }

        public override string ToHtml()
        {
            string ImageHtmlStr = "<img ";
            ImageHtmlStr += ("width=\"" + oneSize.width + "\" ");
            ImageHtmlStr += ("height=\"" + oneSize.height + "\" ");
            ImageHtmlStr += ("src=\"data:image/png;base64," + GetImageBase64() + "\"");
            ImageHtmlStr += "/>";

            return ImageHtmlStr;
        }

        public override void SaveTable()
        {
            throw new NotImplementedException();
        }
    }

    public class OneOCRText
    {
        private string Value;

        public OneOCRText(XElement XElem)
        {
            if (XElem.Value != null)
            {
                Value = XElem.Value;
            }
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
            if (XElem.Attribute("startPos") != null)
            {
                startPos = XElem.Attribute("startPos").Value;
            }
            if (XElem.Attribute("region") != null)
            {
                region = XElem.Attribute("region").Value;
            }
            if (XElem.Attribute("line") != null)
            {
                line = XElem.Attribute("line").Value;
            }
            if (XElem.Attribute("x") != null)
            {
                x = XElem.Attribute("x").Value;
            }
            if (XElem.Attribute("y") != null)
            {
                y = XElem.Attribute("y").Value;
            }
            if (XElem.Attribute("z") != null)
            {
                z = XElem.Attribute("z").Value;
            }
            if (XElem.Attribute("width") != null)
            {
                width = XElem.Attribute("width").Value;
            }
            if (XElem.Attribute("height") != null)
            {
                height = XElem.Attribute("height").Value;
            }
        }
    }

    public class OneOCRData
    {
        private string lang;

        private OneOCRText oneOCRText;
        private List<OneOCRToken> oneOCRTokens = new List<OneOCRToken>();

        public OneOCRData(XElement XElem)
        {
            if (XElem.Attribute("lang") != null)
            {
                lang = XElem.Attribute("lang").Value;
            }

            if (XElem.Element(OneDataHelper.OneSpace + "OCRText") != null)
            {
                oneOCRText = new OneOCRText(XElem.Element(OneDataHelper.OneSpace + "OCRText"));
            }

            if (XElem.Elements(OneDataHelper.OneSpace + "OCRToken") != null)
            {
                foreach (XElement item in XElem.Elements(OneDataHelper.OneSpace + "OCRToken"))
                {
                    if (item != null)
                    {
                        oneOCRTokens.Add(new OneOCRToken(item));
                    }
                }
            }            
        }
    }
}
