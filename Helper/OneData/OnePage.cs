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
        // 属性
        public static XNamespace ns;
        private string id;
        private string name;
        private string dateTime;
        private string lastModifiedTime;
        private string pageLevel;
        private string isCurrentlyViewed;
        private string lang;

        // 标签部分
        private List<OneTagDef> oneTagDefs;
        private List<OneQuickStyleDef> oneQuickStyleDefs;
        private OnePageSettings onePageSettings;
        private OneTitle oneTitle;


        public OnePage(XDocument XDoc)
        {
            // 获取根组件
            XElement RootElem = XDoc.Root;
            ns = RootElem.Name.Namespace;

            // 初始化TagDefs
            foreach (XElement Elem in RootElem.Elements(ns + "TagDef"))
            {
                OneTagDef TmpTagDef = new OneTagDef(Elem);
                oneTagDefs.Add(TmpTagDef);
            }
            // 初始化QuickStyleDefs
            foreach (XElement Elem in RootElem.Elements(ns + "QuickStyleDef"))
            {
                OneQuickStyleDef TmpQuickStyleDef = new OneQuickStyleDef(Elem);
                oneQuickStyleDefs.Add(TmpQuickStyleDef);
            }
            // 初始化PageSettings
            onePageSettings = new OnePageSettings(RootElem.Element(ns + "PageSettings"));
            // 初始化Title
            oneTitle = new OneTitle();
        }
    }
}
