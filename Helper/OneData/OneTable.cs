using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper
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
            if (XElem.Attribute("isLocked") != null)
            {
                isLocked = XElem.Attribute("isLocked").Value;
            }
        }
    }

    class OneColumns
    {
        private List<OneColumn> oneColumns = new List<OneColumn>();

        public OneColumns(XElement XElem)
        {
            foreach (XElement item in XElem.Elements(OneDataHelper.OneSpace + "Column"))
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

            oneOEChildren = new OneOEChildren(XElem.Element(OneDataHelper.OneSpace + "OEChildren"));
        }
    }

    class OneRow
    {
        private string objectID;
        private string lastModifiedTime;
        private List<OneCell> oneCells = new List<OneCell>();

        public OneRow(XElement XElem)
        {
            objectID = XElem.Attribute("objectID").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            foreach (XElement item in XElem.Elements( OneDataHelper.OneSpace + "Cell"))
            {
                oneCells.Add(new OneCell(item));
            }

        }
    }

    class OneTable : OneItem
    {
        private string bordersVisible;
        private string hasHeaderRow;
        private string lastModifiedTime;
        private string objectID;
        private OneColumns oneColumns;
        private List<OneRow> oneRows = new List<OneRow>();



        public OneTable(XElement XElem)
        {
            bordersVisible = XElem.Attribute("bordersVisible").Value;
            hasHeaderRow = XElem.Attribute("hasHeaderRow").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            objectID = XElem.Attribute("objectID").Value;

            oneColumns = new OneColumns(XElem.Element(OneDataHelper.OneSpace + "Columns"));
            foreach (XElement item in XElem.Elements(OneDataHelper.OneSpace + "Row"))
            {
                oneRows.Add(new OneRow(XElem));
            }
        }

        public override string GetStringData()
        {
            throw new NotImplementedException();
        }
    }
}
