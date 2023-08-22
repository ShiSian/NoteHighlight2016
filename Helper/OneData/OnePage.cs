using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    public class OnePage
    {
        private string id;
        private string name;
        private string dateTime;
        private string lastModifiedTime;
        private string pageLevel;
        private string isCurrentlyViewed;
        private string lang;
        private List<QuickStyleDef> quickStyleDefs;

        public OnePage()
        {

        }
        public OnePage(XDocument document)
        {

        }



        public string ID
        {
            get { return id; }
            set { }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        public string LastModifiedTime
        {
            get { return lastModifiedTime; }
            set { lastModifiedTime = value; }
        }
        public string PageLevel
        {
            get { return pageLevel; }
            set { pageLevel = value; }
        }
        public string IsCurrentlyViewed
        {
            get { return isCurrentlyViewed; }
            set { isCurrentlyViewed = value; }
        }
        public string Lang
        {
            get { return lang; }
            set { lang = value; }
        }
        public List<QuickStyleDef> QuickStyleDefs
        {
            get { return quickStyleDefs; }
            set { quickStyleDefs = value; }
        }



    }
}
