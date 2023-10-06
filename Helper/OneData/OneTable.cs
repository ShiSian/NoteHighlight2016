using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    public class OneTable : OneItem
    {
        private string bordersVisible;
        private string hasHeaderRow;
        private string lastModifiedTime;
        private string objectID;
        private OneColumns oneColumns;
        private List<OneRow> oneRows = new List<OneRow>();



        public OneTable(XElement XElem, int InIndentNum)
        {
            IndentNum = InIndentNum;

            bordersVisible = XElem.Attribute("bordersVisible").Value;
            hasHeaderRow = XElem.Attribute("hasHeaderRow").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            objectID = XElem.Attribute("objectID").Value;

            oneColumns = new OneColumns(XElem.Element(OneDataHelper.OneSpace + "Columns"));

            foreach (XElement RowElem in XElem.Elements(OneDataHelper.OneSpace + "Row"))
            {
                OneRow TmpRow = new OneRow(RowElem);
                oneRows.Add(TmpRow);
            }
        }

        public override string ToStr()
        {
            return "OneTable";
        }

        public override string ToCSV()
        {
            string OutStr = "";

            foreach (OneRow TmpRow in oneRows)
            {
                OutStr += TmpRow.ToCSV();
                OutStr += Environment.NewLine;
            }

            OutStr = OutStr.TrimEnd(Environment.NewLine.ToCharArray());

            return OutStr;
        }


        public override string ToHtml()
        {
            // 表格整体设置
            string TableHtmlStr = "<table cellspacing=\"0\" ";
            if (bordersVisible == "true")
            {
                TableHtmlStr += ("border=\"1\" ");
            }
            else
            {
                TableHtmlStr += ("border=\"0\" ");
            }
            TableHtmlStr += (">" + Environment.NewLine);

            // 表格列设置
            TableHtmlStr += oneColumns.ToHtml();;

            // 表格行设置
            string RowsHtmlstr = "";
            foreach (OneRow item in oneRows)
            {
                string RowHtmlStr = item.ToHtml();
                RowsHtmlstr += RowHtmlStr;

                // 保存单行表格到本地
                string TmpOneRowHtmlStr = TableHtmlStr + RowHtmlStr + "</table>" + Environment.NewLine;
                string TableRowTitle;

                try
                {
                    TableRowTitle = item.GetCellContent(0);
                }
                catch (Exception)
                {

                    TableRowTitle = "Failed to get Cell content.";
                }

                string FilePath = @"D:\OneNoteExport\" + TableRowTitle + ".html";
                if (!string.IsNullOrEmpty(TableRowTitle) && TableRowTitle.Length < 256)
                {
                    OneDataHelper.SaveString2File(TmpOneRowHtmlStr, FilePath);
                }

            }
            TableHtmlStr += RowsHtmlstr;

            TableHtmlStr += ("</table>" + Environment.NewLine);
            return TableHtmlStr;
        }
    }
    class OneColumn : OneItem
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

        public override string ToCSV()
        {
            throw new NotImplementedException();
        }

        public override string ToHtml()
        {
            //< col width = 200 >

            float TmpWidth;
            if (float.TryParse(width, out TmpWidth))
            {
                return "<col width=" + TmpWidth*1.3 + ">" + Environment.NewLine;
            }
            else
            {
                return "<col width=" + width + ">" + Environment.NewLine;
            }
        }

        public override string ToStr()
        {
            throw new NotImplementedException();
        }
    }

    class OneColumns : OneItem
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

        public int Num()
        {
            return oneColumns.Count();
        }

        public override string ToCSV()
        {
            throw new NotImplementedException();
        }

        public override string ToHtml()
        {
            string ColumnsHtmlStr = "<colgroup>" + Environment.NewLine;
            foreach (OneColumn item in oneColumns)
            {
                ColumnsHtmlStr += item.ToHtml();
            }
            ColumnsHtmlStr += "</colgroup>" + Environment.NewLine;
            return ColumnsHtmlStr;
        }

        public override string ToStr()
        {
            throw new NotImplementedException();
        }
    }

    class OneCell : OneItem
    {
        private string shadingColor;
        private string lastModifiedTime;
        private string objectID;
        private string lastModifiedByInitials;
        private OneOEChildren oneOEChildren;

        public OneCell(XElement XElem)
        {
            if (XElem.Attribute("shadingColor") != null)
            {
                shadingColor = XElem.Attribute("shadingColor").Value;
            }
            if (XElem.Attribute("lastModifiedTime") != null)
            {
                lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            }
            if (XElem.Attribute("objectID") != null)
            {
                objectID = XElem.Attribute("objectID").Value;
            }
            if (XElem.Attribute("lastModifiedByInitials") != null)
            {
                lastModifiedByInitials = XElem.Attribute("lastModifiedByInitials").Value;
            }

            oneOEChildren = new OneOEChildren(XElem.Element(OneDataHelper.OneSpace + "OEChildren"), -1);
        }

        public override string ToStr()
        {
            return oneOEChildren.ToStr();
        }

        public override string ToCSV()
        {
            string OutStr = "";
            OutStr = oneOEChildren.ToCSV();
            OutStr = OneDataHelper.SubStrIndent(OutStr);
            OutStr = OneDataHelper.UpdateIndent(OutStr);
            OutStr = OutStr.TrimEnd();
            return OutStr;
        }

        public override string ToHtml()
        {
            string OneCellHtmlStr = "<td ";
            if (!string.IsNullOrEmpty(shadingColor))
            {
                //style="background-color:red"
                OneCellHtmlStr += ("style=\"background-color:" + shadingColor + "\"");
            }
            OneCellHtmlStr += ">" + Environment.NewLine;

            string OEChildrenStr = oneOEChildren.ToHtml();
            OneCellHtmlStr += OEChildrenStr;

            OneCellHtmlStr += "</td>" + Environment.NewLine;
            return OneCellHtmlStr;
        }
    }

    public class OneRow : OneItem
    {
        private string objectID;
        private string lastModifiedTime;
        private List<OneCell> oneCells = new List<OneCell>();

        public OneRow(XElement XElem)
        {
            objectID = XElem.Attribute("objectID").Value;
            lastModifiedTime = XElem.Attribute("lastModifiedTime").Value;
            foreach (XElement item in XElem.Elements(OneDataHelper.OneSpace + "Cell"))
            {
                oneCells.Add(new OneCell(item));
            }
        }

        public string GetCellContent(int InCellIndex)
        {
           return oneCells[InCellIndex].ToStr();
        }

        public override string ToStr()
        {
            throw new NotImplementedException();
        }

        public override string ToCSV()
        {
            string OutStr = "";

            foreach (OneCell TmpCell in oneCells)
            {
                OutStr += "\"";

                OutStr += TmpCell.ToCSV();

                OutStr += "\",";
            }

            OutStr = OutStr.TrimEnd(',');

            return OutStr;
        }

        public override string ToHtml()
        {
            string OneRowHtmlStr = "<tr>" + Environment.NewLine;

            foreach (OneCell item in oneCells)
            {
                OneRowHtmlStr += item.ToHtml();
            }

            OneRowHtmlStr += ("</tr>" + Environment.NewLine);
            return OneRowHtmlStr;
        }
    }
}
