using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    public class QuickStyleDef
    {
        public QuickStyleDef()
        {

        }
        public QuickStyleDef(XElement Elem)
        {
            //index = Elem.desa
        }


        private string index;


        public string Index
        {
            get { return index; }
            set { index = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string fontColor;

        public string FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }
        private string highlightColor;

        public string HighlightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        private string font;

        public string Font
        {
            get { return font; }
            set { font = value; }
        }

        private string fontSize;

        public string FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        private string spaceBefore;

        public string SpaceBefore
        {
            get { return spaceBefore; }
            set { spaceBefore = value; }
        }

        private string spaceAfter;

        public string SpaceAfter
        {
            get { return spaceAfter; }
            set { spaceAfter = value; }
        }


    }
}
