using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    class OneColumn
    {
        private string index;
        private string width;
        private string isLocked;

        public OneColumn(XElement XElem)
        {
            index = XElem.Attribute("index").Value;
            width = XElem.Attribute("width").Value;
            isLocked = XElem.Attribute("isLocked").Value;
        }
    }

    class OneColumns
    {
        private List<OneColumn> oneColumns;

        public OneColumns(XElement XElem)
        {
            foreach (XElement item in XElem.Elements(OnePage.ns + "Column"))
            {
                OneColumn TmpOneColumn = new OneColumn(item);
                oneColumns.Add(TmpOneColumn);
            }
        }
    }

    class OneCell
    {
        private string shadingColor;
        private string lastModifiedTime;
        private string objectID;
        private string lastModifiedByInitials;
        private OneOEChildren oneOEChildren;

        public OneCell(XElement XElem)
        {
            shadingColor = XElem.Attribute("shadingColor").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            objectID = XElem.Attribute("objectID").Value;
            lastModifiedByInitials = XElem.Attribute("lastModifiedByInitials").Value;

            oneOEChildren = new OneOEChildren(XElem.Element(OnePage.ns + ""));
        }
    }

    class OneRow
    {
        private string objectID;
        private string lastModifiedTime;

        public OneRow(XElement XElem)
        {
            objectID = XElem.Attribute("objectID").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;

        }
    }

    class OneTable
    {
        private string bordersVisible;
        private string hasHeaderRow;
        private string lastModifiedTime;
        private string objectID;
        private OneColumn oneColumn;



        public OneTable(XElement XElem)
        {
            bordersVisible = XElem.Attribute("bordersVisible").Value;
            hasHeaderRow = XElem.Attribute("hasHeaderRow").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            objectID = XElem.Attribute("objectID").Value;

            oneColumn = new OneColumn(XElem.Element(OnePage.ns + "Columns"));
            foreach (XElement item in XElem.Elements(OnePage.ns + "Row"))
            {

            }

        }


    }
}
