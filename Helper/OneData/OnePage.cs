﻿using Helper.OneData;
using System;
using System.Collections.Generic;
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


    public class OnePage :  OneItem
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
        public static List<OneQuickStyleDef> oneQuickStyleDefs = new List<OneQuickStyleDef>();
        private OnePageSettings onePageSettings;
        private OneTitle oneTitle;
        private List<OneOutline> oneOutlines = new List<OneOutline>();

        // 根据索引获取OneQuickStyleDef
        public static OneQuickStyleDef GetQuickStyleDef(string InStyleIndex)
        {
            // 先尝试根据索引直接查找对应的样式定义
            int TmpStyleIndex;
            if (int.TryParse(InStyleIndex, out TmpStyleIndex))
            {
                OneQuickStyleDef TmpQuickStyleDef = oneQuickStyleDefs[TmpStyleIndex];
                if (TmpQuickStyleDef.index == InStyleIndex)
                {
                    return TmpQuickStyleDef;
                }
            }


            // 获取失败，遍历寻找
            foreach (OneQuickStyleDef TmpQuickStyleDef in oneQuickStyleDefs)
            {
                if (TmpQuickStyleDef.index == InStyleIndex)
                {
                    return TmpQuickStyleDef;
                }
            }

            // 寻找失败，返回空
            return null;
        }


        public OnePage(XDocument XDoc)
        {
            // 获取根组件
            XElement RootElem = XDoc.Root;

            // 元素公用类
            OneDataHelper.UpdateSpace(RootElem.Name.Namespace);

            if (RootElem.Attribute("ID") != null)
            {
                ID = RootElem.Attribute("ID").Value;
            }
            if (RootElem.Attribute("name") != null)
            {
                name = RootElem.Attribute("name").Value;
            }
            if (RootElem.Attribute("dateTime") != null)
            {
                dateTime = RootElem.Attribute("dateTime").Value;
            }
            if (RootElem.Attribute("lastModifiedTime") != null)
            {
                lastModifiedTime = RootElem.Attribute("lastModifiedTime").Value;
            }
            if (RootElem.Attribute("pageLevel") != null)
            {
                pageLevel = RootElem.Attribute("pageLevel").Value;
            }
            if (RootElem.Attribute("isCurrentlyViewed") != null)
            {
                isCurrentlyViewed = RootElem.Attribute("pageLevel").Value;
            }
            if (RootElem.Attribute("lang") != null)
            {
                lang = RootElem.Attribute("lang").Value;
            }


            // 初始化TagDefs
            if (RootElem.Elements(OneDataHelper.OneSpace + "TagDef") != null)
            {
                foreach (XElement Elem in RootElem.Elements(OneDataHelper.OneSpace + "TagDef"))
                {
                    OneTagDef TmpTagDef = new OneTagDef(Elem);
                    oneTagDefs.Add(TmpTagDef);
                }
            }

            // 初始化QuickStyleDefs
            if (RootElem.Elements(OneDataHelper.OneSpace + "QuickStyleDef") != null)
            {
                foreach (XElement Elem in RootElem.Elements(OneDataHelper.OneSpace + "QuickStyleDef"))
                {
                    OneQuickStyleDef TmpQuickStyleDef = new OneQuickStyleDef(Elem);
                    oneQuickStyleDefs.Add(TmpQuickStyleDef);
                }
            }

            // 初始化PageSettings
            if (RootElem.Element(OneDataHelper.OneSpace + "PageSettings") != null)
            {
                onePageSettings = new OnePageSettings(RootElem.Element(OneDataHelper.OneSpace + "PageSettings"));
            }

            // 初始化Title
            if (RootElem.Element(OneDataHelper.OneSpace + "Title") != null)
            {
                oneTitle = new OneTitle(RootElem.Element(OneDataHelper.OneSpace + "Title"));
            }

            // 初始化Outline
            if (RootElem.Elements(OneDataHelper.OneSpace + "Outline") != null)
            {
                foreach (XElement Elem in RootElem.Elements(OneDataHelper.OneSpace + "Outline"))
                {
                    OneOutline TmpOneOutline = new OneOutline(Elem);
                    oneOutlines.Add(TmpOneOutline);
                }
            }

            OneDataHelper.InitCurrentPage(this);
        }


        public override string ToStr()
        {
            return "OnePage String is null.";
        }

        public override string ToCSV()
        {
            string OutStr = "";
            foreach (OneOutline item in oneOutlines)
            {
                OutStr += item.ToCSV();
            }
            OutStr = OneDataHelper.SubStrIndent(OutStr);
            return OneDataHelper.UpdateIndent(OutStr);
        }

        public override string ToHtml()
        {
            /*
             #separator:tab
             #html:true
             #tags column:3
             正面内容	"背面内容"
             */
            return oneOutlines[0].ToHtml();
        }

        //保存页面的表格内容
        public override void SaveTable()
        {
            oneOutlines[0].SaveTable();
        }
    }
}
