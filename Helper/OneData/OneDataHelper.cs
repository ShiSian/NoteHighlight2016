using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper
{
    public class OneDataHelper
    {
        public static XNamespace OneSpace;

        public void UpdateSpace(XNamespace InOneSpace)
        {
            OneSpace = InOneSpace;
        }

        public static void AutoConstructItem( ref List<OneItem> OutOneItemList ,XElement XElem)
        {
            XName TmpName = XElem.Name;

            if (TmpName == (OneSpace + "T"))
            {
                OneT TmpOneT = new OneT(XElem);
                OutOneItemList.Add(TmpOneT);
            }
            else if (TmpName == (OneSpace + "List"))
            {
                OneList TmpOneList = new OneList(XElem);
                OutOneItemList.Add(TmpOneList);
            }
            else if (TmpName == (OneSpace + "OEChildren"))
            {
                OneOEChildren TmpOneOEChildren = new OneOEChildren(XElem);
                OutOneItemList.Add(TmpOneOEChildren);
            }
            else if (TmpName == (OneSpace + "Image"))
            {
                OneImage TmpOneImage = new OneImage(XElem);
                OutOneItemList.Add(TmpOneImage);
            }
            else if (TmpName == (OneSpace + "Table"))
            {
                OneTable TmpOneTable = new OneTable(XElem);
                OutOneItemList.Add(TmpOneTable);
            }
            else
            {

            }
        }
    }
}
