using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    public class OneT
    {
        public OneT() { }
        public OneT(string XmlString)
        {
            XElement element = XElement.Parse(XmlString);
            text = element.Value;
        }
        public OneT(XElement XmlElement)
        {
            text = XmlElement.Value;
        }


        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

    }
}
