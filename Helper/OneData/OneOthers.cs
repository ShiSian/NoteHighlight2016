using Helper.OneData;
using System.Xml.Linq;

namespace Helper
{
    class OneAutomatic
    {
        public OneAutomatic(XElement XElem)
        {

        }
    }

    public class OneQuickStyleDef
    {
        // 属性
        private string index;
        private string name;
        private string fontColor;
        private string highlightColor;
        private string font;
        private string fontSize;
        private string spaceBefore;
        private string spaceAfter;


        public OneQuickStyleDef()
        {

        }

        public OneQuickStyleDef(XElement XElem)
        {
            //index = Elem.desa
            index = XElem.Attribute("index").Value;
            name = XElem.Attribute("name").Value;
            fontColor = XElem.Attribute("fontColor").Value;
            highlightColor = XElem.Attribute("highlightColor").Value;
            font = XElem.Attribute("font").Value;
            fontSize = XElem.Attribute("fontSize").Value;
            spaceBefore = XElem.Attribute("spaceBefore").Value;
            spaceAfter = XElem.Attribute("spaceAfter").Value;
        }

        public string Index
        {
            get { return index; }
            set { index = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }

        public string HighlightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        public string Font
        {
            get { return font; }
            set { font = value; }
        }

        public string FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        public string SpaceBefore
        {
            get { return spaceBefore; }
            set { spaceBefore = value; }
        }

        public string SpaceAfter
        {
            get { return spaceAfter; }
            set { spaceAfter = value; }
        }


    }

    /// <summary>
    /// 【说明】项目标记的定义,OnePage的顶部定义的快速样式
    /// </summary>
    public class OneTagDef
    {
        // 标记索引
        private string index;
        private string type;
        private string symbol;
        private string fontColor;
        private string highlightColor;
        // 标签类型
        private string name;

        public OneTagDef(XElement XElem)
        {
            if (XElem.Attribute("index") != null)
            {
                index = XElem.Attribute("index").Value;
            }
            if (XElem.Attribute("type") != null)
            {
                type = XElem.Attribute("type").Value;
            }
            if (XElem.Attribute("symbol") != null)
            {
                symbol = XElem.Attribute("symbol").Value;
            }
            if (XElem.Attribute("fontColor") != null)
            {
                fontColor = XElem.Attribute("fontColor").Value;
            }
            if (XElem.Attribute("highlightColor") != null)
            {
                highlightColor = XElem.Attribute("highlightColor").Value;
            }
            if (XElem.Attribute("name") != null)
            {
                name = XElem.Attribute("name").Value;
            }
        }

        public string GetStringData()
        {
            if (name == "待办事项")
            {
                return "▢";
            }
            else if (name == "重要")
            {
                return "★";
            }
            else if (name == "问题")
            {
                return "?";
            }
            else
            {
                return name;
            }
        }
    }
}
