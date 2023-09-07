using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper
{
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
}
