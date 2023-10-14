using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    class OneOutline : OneItem
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
        public OneOEChildren oneOEChildren;

        public OneOutline(XElement XElem)
        {
            if (XElem.Attribute("author") != null)
            {
                author = XElem.Attribute("author").Value;
            }
            if (XElem.Attribute("authorInitials") != null)
            {
                authorInitials = XElem.Attribute("authorInitials").Value;
            }
            if (XElem.Attribute("lastModifiedBy") != null)
            {
                lastModifiedBy = XElem.Attribute("lastModifiedBy").Value;
            }
            if (XElem.Attribute("lastModifiedByInitials") != null)
            {
                lastModifiedByInitials = XElem.Attribute("lastModifiedByInitials").Value;
            }
            if (XElem.Attribute("lastModifiedTime") != null)
            {
                lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            }
            if (XElem.Attribute("objectID") != null)
            {
                objectID = XElem.Attribute("objectID").Value;
            }

            if (XElem.Element(OneDataHelper.OneSpace + "Position") != null)
            {
                onePosition = new OnePosition(XElem.Element(OneDataHelper.OneSpace + "Position"));
            }
            if (XElem.Element(OneDataHelper.OneSpace + "Size") != null)
            {
                oneSize = new OneSize(XElem.Element(OneDataHelper.OneSpace + "Size"));
            }
            if (XElem.Element(OneDataHelper.OneSpace + "OEChildren")  != null)
            {
                oneOEChildren = new OneOEChildren(XElem.Element(OneDataHelper.OneSpace + "OEChildren"), -1);
            }
        }


        public override string ToCSV()
        {
            if (oneOEChildren != null)
            {
                return oneOEChildren.ToCSV();
            }
            return "oneOEChildren in OEOutline is null.";
        }

        public override string ToHtml()
        {
            string OutlineHtmlStr = "<div ";
            // 【设置距离左侧和顶部的距离】
            //if (onePosition != null)
            //{
            //    //left: 距离左边距离; top: 距离顶部距离
            //    OutlineHtmlStr += "style=\"position:absolute;";
            //    OutlineHtmlStr += ("left:" + onePosition.x + "px;");
            //    OutlineHtmlStr += ("top:" + onePosition.y + "px;\"");
            //}
            OutlineHtmlStr += (">" + Environment.NewLine);

            if (oneOEChildren != null)
            {
                OutlineHtmlStr += oneOEChildren.ToHtml();
            }

            OutlineHtmlStr += ("</div>" + Environment.NewLine);

            return OutlineHtmlStr;
        }

        public override string ToStr()
        {
            if (oneOEChildren != null)
            {
                return oneOEChildren.ToStr();
            }
            return "oneOEChildren in OEOutline is null.";
        }

        public override void SaveTable()
        {
            oneOEChildren.SaveTable();
        }
    }

    public class OneOEChildren : OneItem
    {
        private List<OneOE> oneOEs = new List<OneOE>();

        public OneOEChildren(XElement XElem, int InIndentNum)
        {
            IndentNum = InIndentNum;
            if (XElem.Elements(OneDataHelper.OneSpace + "OE") != null)
            {
                foreach (XElement item in XElem.Elements(OneDataHelper.OneSpace + "OE"))
                {
                    OneOE TmpOneOE = new OneOE(item, IndentNum + 1);
                    oneOEs.Add(TmpOneOE);
                }
            }
        }

        public override string ToStr()
        {
            string OEChildrenStr = "";

            foreach (OneOE item in oneOEs)
            {
                OEChildrenStr += item.ToStr();
            }

            return OEChildrenStr;
        }


        public override string ToCSV()
        {
            string OutStr = "";

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

        public override string ToHtml()
        {
            string OEChildrenHtmlStr = "";
            foreach (OneOE TmpOneOE in oneOEs)
            {
                OEChildrenHtmlStr += TmpOneOE.ToHtml();
            }
            return OEChildrenHtmlStr;
        }

        public override void SaveTable()
        {
            foreach (OneOE TmpOneOE in oneOEs)
            {
                TmpOneOE.SaveTable();
            }
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
        private string style;                                 // 该字段对OE内容的样式进行了说明，不是所有的OE都有该字段

        private OneQuickStyleDef oneQuickStyleDef;            // OE对象对应的快速样式定义
        public List<OneItem> oneItems = new List<OneItem>();  // 元素部分（灵活变化部分，OE的子元素可能是OneOEChildren、OneT、OneList等）


        public OneOE(XElement XElem, int InIndentNum)
        {
            IndentNum = InIndentNum;

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
            if (XElem.Attribute("lastModifiedByResolutionID") != null)
            {
                lastModifiedByResolutionID = XElem.Attribute("lastModifiedByResolutionID").Value;
            }
            if (XElem.Attribute("creationTime") != null)
            {
                creationTime = XElem.Attribute("creationTime").Value;
            }
            if (XElem.Attribute("lastModifiedTime") != null)
            {
                lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            }
            if (XElem.Attribute("objectID") != null)
            {
                objectID = XElem.Attribute("objectID").Value;
            }
            if (XElem.Attribute("alignment") != null)
            {
                alignment = XElem.Attribute("alignment").Value;
            }
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
            if (XElem.Attribute("style") != null)
            {
                style = XElem.Attribute("style").Value;
            }


            if (XElem.Elements() != null)
            {
                foreach (XElement item in XElem.Elements())
                {
                    OneDataHelper.AutoConstructItem(ref oneItems, item, IndentNum);
                }
            }

        }

        public override string ToStr()
        {
            string OEStr = "";

            foreach (OneItem item in oneItems)
            {
                OEStr += item.ToStr();
            }

            return OEStr;
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

        // 判断元素是否还有项目符号或项目编号
        private bool ContainListItem()
        {
            foreach (OneItem item in oneItems)
            {
                if (item is OneList || item is OneTag)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToHtml()
        {
            // 先获取该对象对应的QuickStyleDef
            if (oneQuickStyleDef is null)
            {
                if (!string.IsNullOrWhiteSpace(quickStyleIndex))
                {
                    oneQuickStyleDef = OnePage.GetQuickStyleDef(quickStyleIndex);
                }
            }    


            string TmpStyle = "style=\"position:relative;word-wrap:break-word;";
            if (!string.IsNullOrEmpty(style))
            {
                TmpStyle += (style + ";");
            }
            if (spaceBefore != null)
            {
                TmpStyle += ("margin-top:" + spaceBefore + "pt;");
            }
            if (spaceAfter != null)
            {
                TmpStyle += ("margin-bottom:" + spaceAfter + "pt;");
            }
            if (alignment != null)
            {
                TmpStyle += ("text-align:" + alignment + ";");
            }
            // 应用对象的快速样式定义中内容(如果该对象内已经对某个属性进行了定义就不在应用快速样式定义中的内容)
            if (!(oneQuickStyleDef is null))
            {
                if (!TmpStyle.Contains("color") && !string.IsNullOrEmpty(oneQuickStyleDef.fontColor))
                {
                    TmpStyle += ("color:" + oneQuickStyleDef.fontColor + ";");
                }
                if (!TmpStyle.Contains("highlightColor") && !string.IsNullOrEmpty(oneQuickStyleDef.highlightColor))
                {
                    TmpStyle += ("highlightColor:" + oneQuickStyleDef.highlightColor + ";");
                }
                if (!TmpStyle.Contains("font-family") && !string.IsNullOrEmpty(oneQuickStyleDef.font))
                {
                    TmpStyle += ("font-family:" + oneQuickStyleDef.font + ";");
                }
                if (!TmpStyle.Contains("font-size") && !string.IsNullOrEmpty(oneQuickStyleDef.fontSize))
                {
                    TmpStyle += ("font-size:" + oneQuickStyleDef.fontSize + "pt;");
                }
                if (!TmpStyle.Contains("margin-top") && !string.IsNullOrEmpty(oneQuickStyleDef.spaceBefore))
                {
                    TmpStyle += ("margin-top:" + oneQuickStyleDef.spaceBefore + "pt;");
                }
                if (!TmpStyle.Contains("margin-bottom") && !string.IsNullOrEmpty(oneQuickStyleDef.spaceAfter))
                {
                    TmpStyle += ("margin-bottom:" + oneQuickStyleDef.spaceAfter + "pt;");
                }
            }
            // 当没有项目符号或项目编号时需要额外缩进两个字节
            if (IndentNum != 0)
            {                
                if (ContainListItem())
                {
                    TmpStyle += "text-indent:-1em;padding-left: 2em;";
                }
                else
                {
                    TmpStyle += "text-indent:0em;padding-left: 2.5em;";
                }
            }
            TmpStyle += "\"";
            string OEHtmlStr = "<div " + TmpStyle + ">" + Environment.NewLine;


            OneOEChildren oneOEChildren = null;
            foreach (OneItem item in oneItems)
            {
                if (item is OneOEChildren)
                {
                    oneOEChildren = (OneOEChildren)item;
                }
                else
                {
                    OEHtmlStr += item.ToHtml();
                }
            }


            if (oneOEChildren != null)
            {
                OEHtmlStr += oneOEChildren.ToHtml();
            }

            OEHtmlStr += ("</div>" + Environment.NewLine);
            return OEHtmlStr;
        }


        public override void SaveTable()
        {
            foreach (OneItem item in oneItems)
            {
                item.SaveTable();    
            }
        }
    }

    
    class OnePosition
    {
        public string x;
        public string y;
        public string z;

        public OnePosition(XElement XElem)
        {
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
        }
    }

    class OneSize
    {
        public string width;
        public string height;
        public string isSetByUser;

        public OneSize(XElement XElem)
        {
            if (XElem.Attribute("width") != null)
            {
                width = XElem.Attribute("width").Value;
            }
            if (XElem.Attribute("height") != null)
            {
                height = XElem.Attribute("height").Value;
            }
            if (XElem.Attribute("isSetByUser") != null)
            {
                isSetByUser = XElem.Attribute("isSetByUser").Value;
            }
        }
    }

    
}
