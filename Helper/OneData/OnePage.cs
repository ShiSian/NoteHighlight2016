using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper
{
    public class OneRuleLines
    {
        private string visible;

        public OneRuleLines(XElement XElem)
        {
            visible = XElem.Attribute("visible").Value;
        }
    }
    class OnePageSize
    {
        private OneAutomatic autoMatice;
        public OnePageSize(XElement XElem)
        {
            autoMatice = new OneAutomatic(XElem);
        }
    }

    class OnePageSettings
    {
        private string RTL;
        private string color;
        private OnePageSize onePageSize;
        private OneRuleLines oneRuleLines;


        public OnePageSettings(XElement XElem)
        {
            RTL = XElem.Attribute("RTL").Value;
            color = XElem.Attribute("color").Value;
            onePageSize = new OnePageSize(XElem.Element(OneDataHelper.OneSpace + "PageSize"));
            oneRuleLines = new OneRuleLines(XElem.Element(OneDataHelper.OneSpace + "RuleLines"));
        }
    }


    public class OnePage
    {
        // 属性
        private string ID;
        private string name;
        private string dateTime;
        private string lastModifiedTime;
        private string pageLevel;
        private string isCurrentlyViewed;
        private string lang;

        // 标签部分
        private List<OneTagDef> oneTagDefs = new List<OneTagDef>();
        private List<OneQuickStyleDef> oneQuickStyleDefs = new List<OneQuickStyleDef>();
        private OnePageSettings onePageSettings;
        private OneTitle oneTitle;
        private List<OneOutline> oneOutlines = new List<OneOutline>();

        // 其他字段
        public OneDataHelper oneDataHelper;

        //public OnePage() { }
        public OnePage(XDocument XDoc)
        {
            // 获取根组件
            XElement RootElem = XDoc.Root;
            Console.WriteLine("New OnePage");

            // 元素公用类
            oneDataHelper = new OneDataHelper();
            oneDataHelper.UpdateSpace(RootElem.Name.Namespace);

            ID = RootElem.Attribute("ID").Value;
            name = RootElem.Attribute("name").Value;
            dateTime = RootElem.Attribute("dateTime").Value;
            lastModifiedTime = RootElem.Attribute("lastModifiedTime").Value;
            pageLevel = RootElem.Attribute("pageLevel").Value;
            isCurrentlyViewed = RootElem.Attribute("pageLevel").Value;
            lang = RootElem.Attribute("lang").Value;


            // 初始化TagDefs
            foreach (XElement Elem in RootElem.Elements(OneDataHelper.OneSpace + "TagDef"))
            {
                OneTagDef TmpTagDef = new OneTagDef(Elem);
                oneTagDefs.Add(TmpTagDef);
            }
            // 初始化QuickStyleDefs
            foreach (XElement Elem in RootElem.Elements(OneDataHelper.OneSpace + "QuickStyleDef"))
            {
                OneQuickStyleDef TmpQuickStyleDef = new OneQuickStyleDef(Elem);
                oneQuickStyleDefs.Add(TmpQuickStyleDef);
            }
            // 初始化PageSettings
            onePageSettings = new OnePageSettings(RootElem.Element(OneDataHelper.OneSpace + "PageSettings"));
            
            // 初始化Title
            oneTitle = new OneTitle(RootElem.Element(OneDataHelper.OneSpace + "Title"));

            // 初始化Outline
            foreach (XElement Elem in RootElem.Elements(OneDataHelper.OneSpace + "Outline"))
            {
                OneOutline TmpOneOutline = new OneOutline(Elem);
                oneOutlines.Add(TmpOneOutline);
            }
        }
    }
}
