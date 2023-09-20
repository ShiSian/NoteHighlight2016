using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper
{
    public abstract class OneItem
    {
        abstract public string GetStringData();
        abstract public string ToCSV();
    }

    public class OneSpan : OneItem
    {
        private string lang;
        private string value;

        public OneSpan(XElement XElem)
        {
            if (XElem.Attribute("lang") != null)
            {
                lang = XElem.Attribute("lang").Value;
            }
            value = XElem.Value;
        }


        public override string GetStringData()
        {
            return value;
        }

        public override string ToCSV()
        {
            return value;
        }
    }

    public class OneT : OneItem
    {
        private string style;
        private string value;
        private List<OneSpan> oneSpans = new List<OneSpan>();

        public OneT(XElement XmlElement)
        {
            if (XmlElement.Attribute("style") != null)
            {
                style = XmlElement.Attribute("style").Value;
            }
            value = XmlElement.Value;

            // 若文本被拆分进行解析
            if (value.Contains("<span"))
            {
                value = value.Replace("<span\n", "<span ");
                value = value.Replace("lang=en-US", "lang=\"en-US\"");
                value = value.Replace("lang=zh-CN", "lang=\"zh-CN\"");

                foreach (string TmpStr in value.Split(new[] { "</span>" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string fullSpan = TmpStr + "</span>";
                    try
                    {
                        XElement xElement = XElement.Parse(fullSpan);
                        oneSpans.Add(new OneSpan(xElement));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }                    
                }
            }
        }

        public override string GetStringData()
        {
            return value;
        }

        public override string ToCSV()
        {
            string OutStr = "";
            if (oneSpans.Count() > 0)
            {
                foreach (OneSpan oneSpan in oneSpans)
                {
                    OutStr += oneSpan.ToCSV();
                }
            }
            else
            {
                OutStr = value;
            }
            return OutStr + Environment.NewLine;
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

        public override string ToCSV()
        {
            return text;
        }
    }

    public class OneBullet : OneItem
    {
        private string bullet;
        private string fontColor;
        private string fontSize;

        public OneBullet(XElement XElem)
        {
            if (XElem.Attribute("bullet") != null)
            {
                bullet = XElem.Attribute("bullet").Value;
            }
            if (XElem.Attribute("fontColor") != null)
            {
                fontColor = XElem.Attribute("fontColor").Value;
            }
            if (XElem.Attribute("fontSize") != null)
            {
                fontSize = XElem.Attribute("fontSize").Value;
            }
        }

        public override string GetStringData()
        {
            throw new NotImplementedException();
        }

        public override string ToCSV()
        {
            switch (bullet)
            {
                case "0":
                    return "⚫";
                case "1":
                    return "●";
                case "2":
                    return "⦁";
                case "3":
                    return "○";
                case "4":
                    return "⦿";
                case "5":
                    return "❂";

                case "6":
                    return "✧";
                case "7":
                    return "◇";
                case "8":
                    return "◆";
                case "9":
                    return "♦";
                case "10":
                    return "❖";

                case "11":
                    return "▪";
                case "12":
                    return "▫";
                case "13":
                    return "■";
                case "14":
                    return "□";

                case "15":
                    return "▸";
                case "16":
                    return "▶";
                case "17":
                    return "→";
                case "18":
                    return "⇒";
                case "19":
                    return ">";
                case "20":
                    return "➣";

                case "21":
                    return "✱";
                case "22":
                    return "✶";
                case "23":
                    return "✸";
                case "24":
                    return "✺";

                case "25":
                    return "—";
                case "26":
                    return "——";
                case "27":
                    return "———";


                case "28":
                    return "🙂";
                case "29":
                    return "😐";
                case "30":
                    return "😑";

                case "31":
                    return "✔";
                case "32":
                    return "☎";
                case "33":
                    return "✉";

                default:
                    return "——NotFound——";
            }
        }
    }

    public class OneList : OneItem
    {
        private OneNumber oneNumber;
        private OneBullet oneBullet;

        public OneList(XElement XElem)
        {
            oneNumber = new OneNumber(XElem.Element(OneDataHelper.OneSpace + "Number"));
            oneBullet = new OneBullet(XElem.Element(OneDataHelper.OneSpace + "Bullet"));
        }

        public override string GetStringData()
        {
            return oneNumber.GetStringData();
        }

        public override string ToCSV()
        {
            string OutStr = oneNumber.ToCSV();
            if (string.IsNullOrEmpty(OutStr))
            {
                return oneBullet.ToCSV();
            }
            return OutStr + Environment.NewLine;
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

        // 元素部分（灵活变化部分，OE的子元素可能是OneOEChildren、OneT、OneList等）
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

        public override string ToCSV()
        {
            string OutStr = "";
            foreach (OneItem item in oneItems)
            {
                OutStr += item.ToCSV();
            }
            return OutStr;
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

        //private string UpdateIndent(string InStr)
        //{
        //    string Pattern = @"(";
        //    Pattern += OneDataHelper.IndentSymbol;
        //    Pattern += @")+";

        //    Regex r = new Regex(Pattern);
        //    MatchEvaluator myEvaluator = new MatchEvaluator(this.AddIndent);
        //    return r.Replace(InStr, myEvaluator);
        //}

        //public string AddIndent(Match m)
        //{
        //    return m.ToString() + OneDataHelper.IndentSymbol;
        //}



        public override string ToCSV()
        {
            string OutStr = "";

            //string Pattern = @"(" + OneDataHelper.IndentSymbol + @")+";
            //Regex r = new Regex(Pattern);
            //MatchEvaluator AddEvaluator = new MatchEvaluator(this.AddIndent);


            foreach (OneOE TmpOneOE in oneOEs)
            {
                // 为每个子项增加缩进
                string TmpStr = TmpOneOE.ToCSV();
                //TmpStr = r.Replace(TmpStr, AddEvaluator);
                TmpStr = OneDataHelper.IndentSymbol + OneDataHelper.AddStrIndent(TmpStr);
                OutStr += TmpStr;
            }
            return OutStr;
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

        public override string ToCSV()
        {
            throw new NotImplementedException();
        }
    }

    class OneColumn
    {
        private string index;
        private string width;
        private string isLocked;

        public OneColumn(XElement XElem)
        {
            index = XElem.Attribute("index").Value;
            width = XElem.Attribute("width").Value;
            if (XElem.Attribute("isLocked") != null)
            {
                isLocked = XElem.Attribute("isLocked").Value;
            }
        }
    }

    class OneColumns
    {
        private List<OneColumn> oneColumns = new List<OneColumn>();

        public OneColumns(XElement XElem)
        {
            foreach (XElement item in XElem.Elements(OneDataHelper.OneSpace + "Column"))
            {
                OneColumn TmpOneColumn = new OneColumn(item);
                oneColumns.Add(TmpOneColumn);
            }
        }

        public int Num()
        {
            return oneColumns.Count();
        }
    }

    class OneCell : OneItem
    {
        private string shadingColor;
        private string lastModifiedTime;
        private string objectID;
        private string lastModifiedByInitials;
        private OneOEChildren oneOEChildren;

        public OneCell(XElement XElem)
        {
            if (XElem.Attribute("shadingColor") != null)
            {
                shadingColor = XElem.Attribute("shadingColor").Value;
            }
            if (XElem.Attribute("lastModifiedTime") != null)
            {
                lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            }
            if (XElem.Attribute("objectID") != null)
            {
                objectID = XElem.Attribute("objectID").Value;
            }
            if (XElem.Attribute("lastModifiedByInitials") != null)
            {
                lastModifiedByInitials = XElem.Attribute("lastModifiedByInitials").Value;
            }

            oneOEChildren = new OneOEChildren(XElem.Element(OneDataHelper.OneSpace + "OEChildren"));
        }

        public override string GetStringData()
        {
            throw new NotImplementedException();
        }

        public override string ToCSV()
        {
            string OutStr = "";
            OutStr = oneOEChildren.ToCSV();
            OutStr = OneDataHelper.SubStrIndent(OutStr);
            OutStr = OneDataHelper.UpdateIndent(OutStr);
            OutStr = OutStr.TrimEnd();
            return OutStr;
        }
    }

    class OneRow : OneItem
    {
        private string objectID;
        private string lastModifiedTime;
        private List<OneCell> oneCells = new List<OneCell>();

        public OneRow(XElement XElem)
        {
            objectID = XElem.Attribute("objectID").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            foreach (XElement item in XElem.Elements(OneDataHelper.OneSpace + "Cell"))
            {
                oneCells.Add(new OneCell(item));
            }

        }

        public override string GetStringData()
        {
            throw new NotImplementedException();
        }

        public override string ToCSV()
        {
            string OutStr = "";

            foreach (OneCell TmpCell in oneCells)
            {
                OutStr += "\"";

                OutStr += TmpCell.ToCSV();

                OutStr += "\",";
            }

            OutStr = OutStr.TrimEnd(',');

            return OutStr;
        }
    }

    class OneTable : OneItem
    {
        private string bordersVisible;
        private string hasHeaderRow;
        private string lastModifiedTime;
        private string objectID;
        private OneColumns oneColumns;
        private List<OneRow> oneRows = new List<OneRow>();



        public OneTable(XElement XElem)
        {
            bordersVisible = XElem.Attribute("bordersVisible").Value;
            hasHeaderRow = XElem.Attribute("hasHeaderRow").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            objectID = XElem.Attribute("objectID").Value;

            oneColumns = new OneColumns(XElem.Element(OneDataHelper.OneSpace + "Columns"));

            foreach (XElement RowElem in XElem.Elements(OneDataHelper.OneSpace + "Row"))
            {
                OneRow TmpRow = new OneRow(RowElem);
                oneRows.Add(TmpRow);
            }
        }

        public override string GetStringData()
        {
            throw new NotImplementedException();
        }

        public override string ToCSV()
        {
            string OutStr = "";

            foreach (OneRow TmpRow in oneRows)
            {
                OutStr += TmpRow.ToCSV();
                OutStr += Environment.NewLine;
            }

            OutStr = OutStr.TrimEnd(Environment.NewLine.ToCharArray());

            return OutStr;
        }

        public string ToHTML()
        {


            return "";
        }
    }
}
