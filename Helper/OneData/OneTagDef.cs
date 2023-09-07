using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper
{
    /// <summary>
    /// 项目标记的定义
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
            index = XElem.Attribute("index").Value;
            type = XElem.Attribute("type").Value;
            symbol = XElem.Attribute("symbol").Value;
            fontColor = XElem.Attribute("fontColor").Value;
            highlightColor = XElem.Attribute("highlightColor").Value;
            name = XElem.Attribute("name").Value;
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
