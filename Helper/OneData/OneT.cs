using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    public class OneTitle
    {
        // 属性部分
        private string lang;
        private OneOE oneOE;


        // 构造函数
        public OneTitle(XElement XElem)
        {
            if (XElem.Attribute("lang") != null)
            {
                lang = XElem.Attribute("lang").Value;
            }
            if (XElem.Element(OneDataHelper.OneSpace + "OE") != null)
            {
                oneOE = new OneOE(XElem.Element(OneDataHelper.OneSpace + "OE"), 0);
            }
        }
    }


    /// <summary>
    /// 【说明】OneNote的文本对象
    /// 【解释】
    /// 【示例】
    /// </summary>
    public class OneT : OneItem
    {
        private string style;
        private string value;
        private List<OneSpan> oneSpans = new List<OneSpan>();

        public OneT(XElement XmlElement, int InIndentNum)
        {
            IndentNum = InIndentNum;

            if (XmlElement.Attribute("style") != null)
            {
                style = XmlElement.Attribute("style").Value;
            }

            value = XmlElement.Value;

            string TmpValue = value;
            if (TmpValue.Contains("<span"))
            {
                TmpValue = TmpValue.Replace("<span\n", "<span ");
                TmpValue = TmpValue.Replace("lang=en-US", "lang=\"en-US\"");
                TmpValue = TmpValue.Replace("lang=zh-CN", "lang=\"zh-CN\"");
            }                

            // 初始化为多个span对象
            foreach (string TmpStr in TmpValue.Split(new[] { "</span>" }, StringSplitOptions.RemoveEmptyEntries))
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

        public override string ToStr()
        {
            string OneTStr = "";

            foreach (OneSpan item in oneSpans)
            {
                OneTStr += item.ToStr();
            }

            return OneTStr;
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

        public override string ToHtml()
        {
            return "<span style=\"" + style + "\">" + value + "</span>" + Environment.NewLine;
        }

        public override void SaveTable()
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 【说明】 项目符号和编号
    /// </summary>
    public class OneList : OneItem
    {
        private OneNumber oneNumber;
        private OneBullet oneBullet;

        public OneList(XElement XElem, int InIndentNum)
        {
            IndentNum = InIndentNum;

            if (XElem.Element(OneDataHelper.OneSpace + "Number") != null)
            {
                oneNumber = new OneNumber(XElem.Element(OneDataHelper.OneSpace + "Number"));
            }
            if (XElem.Element(OneDataHelper.OneSpace + "Bullet") != null)
            {
                oneBullet = new OneBullet(XElem.Element(OneDataHelper.OneSpace + "Bullet"));
            }
        }

        public override string ToStr()
        {
            string ListStr = "";
            if (oneNumber != null)
            {
                ListStr += oneNumber.ToStr();
            }
            else if (oneBullet != null)
            {
                ListStr += oneBullet.ToStr();
            }
            return ListStr;
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

        public override string ToHtml()
        {
            string ListHtmlStr = "";
            if (oneNumber != null)
            {
                ListHtmlStr += oneNumber.ToHtml();
            }
            else if(oneBullet != null)
            {
                ListHtmlStr += oneBullet.ToHtml();
            }

            return ListHtmlStr;
        }

        public override void SaveTable()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 【说明】 项目编号
    /// </summary>
    public class OneNumber : OneItem
    {
        private string numberSequence;
        private string numberFormat;
        private string fontColor;
        private string fontSize;
        private string font;
        private string language;
        private string text;
        private string bold;

        public OneNumber(XElement XElem)
        {
            if (XElem != null)
            {
                if (XElem.Attribute("numberSequence") != null)
                {
                    numberSequence = XElem.Attribute("numberSequence").Value;
                }
                if (XElem.Attribute("numberFormat") != null)
                {
                    numberFormat = XElem.Attribute("numberFormat").Value;
                }
                if (XElem.Attribute("fontColor") != null)
                {
                    fontColor = XElem.Attribute("fontColor").Value;
                }
                if (XElem.Attribute("fontSize") != null)
                {
                    fontSize = XElem.Attribute("fontSize").Value;
                }
                if (XElem.Attribute("font") != null)
                {
                    font = XElem.Attribute("font").Value;
                }
                if (XElem.Attribute("language") != null)
                {
                    language = XElem.Attribute("language").Value;
                }
                if (XElem.Attribute("text") != null)
                {
                    text = XElem.Attribute("text").Value;
                }
                if (XElem.Attribute("bold") != null)
                {
                    bold = XElem.Attribute("bold").Value;
                }
            }
        }

        public override string ToStr()
        {
            return text;
        }

        public override string ToCSV()
        {
            return text;
        }

        public override string ToHtml()
        {
            string NumberHtmlStr = "";

            if (!string.IsNullOrEmpty(text))
            {
                string style = "style=\"white-space:pre-wrap;";
                if (!string.IsNullOrEmpty(font))
                {
                    style += ("font-family:" + font + ";");
                }
                if (!string.IsNullOrEmpty(fontSize))
                {
                    style += ("font-size:" + fontSize + "pt;");
                }
                if (!string.IsNullOrEmpty(fontColor))
                {
                    style += ("color:" + fontColor + ";");
                }
                style += "\"";

                NumberHtmlStr = "<span " + style + ">" + text.Replace(" ","") + "</span>" + Environment.NewLine;
            }


            return NumberHtmlStr;
        }

        public override void SaveTable()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 【说明】项目符号
    /// </summary>
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

        public override string ToStr()
        {
            return BulletToStr();
        }

        private string BulletToStr()
        {
            switch (bullet)
            {
                case "0":
                    return "  ●";
                case "1":      
                    return "  ●";
                case "2":
                    return "  ⦁";
                case "3":       
                    return "  ○";
                case "4":       
                    return " ⦿";
                case "5":       
                    return " ❂";
                case "6":       
                    return " ✧";
                case "7":       
                    return "  ◇";
                case "8":       
                    return "  ◆";
                case "9":       
                    return "  ♦";
                case "10":      
                    return " ❖";
                case "11":      
                    return "  ▪";
                case "12":      
                    return "  ▫";
                case "13":      
                    return "  ■";
                case "14":      
                    return "  □";
                case "15":      
                    return "  ▸";
                case "16":      
                    return "  ▶";
                case "17":      
                    return "  →";
                case "18":      
                    return " ⇒";
                case "19":      
                    return "  >";
                case "20":      
                    return " ➣";
                case "21":      
                    return " ✱";
                case "22":      
                    return "  ✶";
                case "23":      
                    return " ✸";
                case "24":      
                    return " ✺";
                case "25":      
                    return "  —";
                case "26":      
                    return " ——";
                case "27":      
                    return "———";
                case "28":      
                    return " 🙂";
                case "29":     
                    return " 😐";
                case "30":      
                    return " 😑";
                case "31":      
                    return "  ✔";
                case "32":      
                    return "  ☎";
                case "33":      
                    return "  ✉";

                default:
                    return " NTF";
            }
        }

        public override string ToCSV()
        {
            return BulletToStr();
        }

        public override string ToHtml()
        {
            //string style = "style=\"";
            //style += ("font-size:" + fontSize + "pt;");
            //style += ("color:" + fontColor + ";\"");
            //return @"<span " + style + @">" + BulletToStr() + @"</span>";

            string BulletHtmlStr = "";

            if (!string.IsNullOrEmpty(bullet))
            {
                string style = "style=\"white-space: pre-wrap;";
                if (!string.IsNullOrEmpty(fontSize))
                {
                    style += ("font-size:" + fontSize + "pt;");
                }
                if (!string.IsNullOrEmpty(fontColor))
                {
                    style += ("color:" + fontColor + ";");
                }
                style += "\"";

                BulletHtmlStr = "<span " + style + ">" + BulletToStr() + @"</span>" + Environment.NewLine;
            }


            return BulletHtmlStr;
        }

        public override void SaveTable()
        {
            throw new NotImplementedException();
        }
    }

    public class OneTag : OneItem
    {
        private string index;
        private string completed;
        private string disabled;
        private string creationDate;


        public OneTag(XElement XElem)
        {
            if (XElem.Attribute("index") != null)
            {
                index = XElem.Attribute("index").Value;
            }
            if (XElem.Attribute("completed") != null)
            {
                completed = XElem.Attribute("completed").Value;
            }
            if (XElem.Attribute("disabled") != null)
            {
                disabled = XElem.Attribute("disabled").Value;
            }
            if (XElem.Attribute("creationDate") != null)
            {
                creationDate = XElem.Attribute("creationDate").Value;
            }
        }

        private string TagToStr()
        {
            switch (index)
            {
                case "0":
                    if (completed == "false")
                    {
                        return "🔲";
                    }
                    return "🔳";
                case "1":
                    return " ★";
                case "2":
                    return "  ?";
                case "3": 
                    return "   ";
                case "4":
                    return "   ";
                case "5":
                    return " ✎";
                case "6":
                    return "📨";
                case "7":
                    return "🏠";
                case "8":
                    return "📞";
                case "9":
                    return "🔗";
                case "10":
                    return " 💡";
                case "11":
                    return " 🔒";
                case "12":
                    return "  ❗";
                case "13":
                    return " 📔";
                case "14":
                    return "📖";
                case "15":
                    return "🎦";
                case "16":
                    return "📑";
                case "17":
                    return "  ♪";
                case "18":
                    return "🔗";
                case "19":     
                    return "💬";
                case "20":     
                    return " 👤";
                case "21":     
                    return " 👤";
                case "22":     
                    return " 👤";
                case "23":     
                    return "📦";
                case "24":     
                    return "💼";
                case "25":     
                    return "💼";
                case "26":     
                    return "💼";
                case "27":
                    return "💼";
                case "28":
                    return "👳";
                default:
                    return "NTF";
            }
        }





        public override string ToCSV()
        {
            throw new NotImplementedException();
        }

        public override string ToHtml()
        {
            return "<span style=\"white-space:pre-wrap;\">" + TagToStr() + "</span>" + Environment.NewLine;
        }

        public override string ToStr()
        {
            return TagToStr();
        }

        public override void SaveTable()
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 【说明】处理OneT中的文本的对象
    /// 【解释】OneT中的文字会根据中文和英文封装在不同的span中，将OneT中每个Span抽象为一个对象
    /// 【示例】<span lang=zh-CN>文本</span><span lang = en - US > T </ span >
    /// </summary>
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


        public override string ToStr()
        {
            return value;
        }

        public override string ToCSV()
        {
            return value;
        }

        public override string ToHtml()
        {
            return value;
        }

        public override void SaveTable()
        {
            throw new NotImplementedException();
        }
    }
}
