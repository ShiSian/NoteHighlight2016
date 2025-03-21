using Helper.OneData;
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
        public int IndentNum;
        abstract public string ToStr();
        abstract public string ToCSV();
        abstract public string ToHtml();
        abstract public void SaveTable();
    }
    

    

    public class OneCallbackID
    {
        private string callbackID;

        public OneCallbackID(XElement XElem)
        {
            callbackID = XElem.Attribute("callbackID").Value;
        }

        public string GetCallbackID()
        {
            return callbackID;
        }
    }

    

    

    
}
