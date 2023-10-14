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
        public string index;
        public string name;
        public string fontColor;
        public string highlightColor;
        public string font;
        public string fontSize;
        public string spaceBefore;
        public string spaceAfter;


        public OneQuickStyleDef()
        {

        }

        public OneQuickStyleDef(XElement XElem)
        {
            if (XElem.Attribute("index").Value != null)
            {
                index = XElem.Attribute("index").Value;
            }
            if (XElem.Attribute("name").Value != null)
            {
                name = XElem.Attribute("name").Value;
            }
            if (XElem.Attribute("fontColor").Value != null)
            {
                fontColor = XElem.Attribute("fontColor").Value;
            }
            if (XElem.Attribute("highlightColor").Value != null)
            {
                highlightColor = XElem.Attribute("highlightColor").Value;
            }
            if (XElem.Attribute("font").Value != null)
            {
                font = XElem.Attribute("font").Value;
            }
            if (XElem.Attribute("fontSize").Value != null)
            {
                fontSize = XElem.Attribute("fontSize").Value;
            }
            if (XElem.Attribute("spaceBefore").Value != null)
            {
                spaceBefore = XElem.Attribute("spaceBefore").Value;
            }
            if (XElem.Attribute("spaceAfter").Value != null)
            {
                spaceAfter = XElem.Attribute("spaceAfter").Value;
            }
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
