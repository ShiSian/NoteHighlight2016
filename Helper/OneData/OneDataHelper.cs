using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Helper
{
    public class OneDataHelper
    {
        public static XNamespace OneSpace;
        public static string IndentSymbol;

        public OneDataHelper()
        {
            IndentSymbol = "iiii" + Guid.NewGuid().ToString() + "iiii";
        }

        // 增加缩进
        public static string AddStrIndent(string InStr)
        {
            string Pattern = @"(" + OneDataHelper.IndentSymbol + @")+";
            Regex r = new Regex(Pattern);
            MatchEvaluator AddEvaluator = new MatchEvaluator(AddIndent);

            InStr = r.Replace(InStr, AddEvaluator);

            return InStr;
        }

        // 减少缩进
        public static string SubStrIndent(string InStr)
        {
            // 去除多余的Indent
            string Pattern = @"(" + OneDataHelper.IndentSymbol + @")+";
            Regex r = new Regex(Pattern);
            MatchEvaluator SubEvaluator = new MatchEvaluator(SubIndent);
            InStr = r.Replace(InStr, SubEvaluator);
            return InStr;
        }

        // 更新缩进
        public static string UpdateIndent(string InStr)
        {
            return InStr.Replace(OneDataHelper.IndentSymbol, "    ");
        }

        // 导出CSV
        public static void SavePage2CSV(string InPageString, string InTargetFilePath)
        {
            XDocument Doc = XDocument.Parse(InPageString);
            if (Doc != null)
            {
                OnePage TmpPage = new OnePage(Doc);
                string OutCSV = TmpPage.ToCSV();

                using(StreamWriter TmpWriter = new StreamWriter(InTargetFilePath))
                {
                    TmpWriter.Write(OutCSV);
                }

                //File.WriteAllText(InTargetFilePath, OutCSV, Encoding.UTF8);
            }
        }

        // 导出XML
        public static void SavePage2XML(string InPageString, string InTargetFilePath)
        {
            XDocument Doc = XDocument.Parse(InPageString);
            if (Doc != null)
            {
                Doc.Save(InTargetFilePath);
            }
        }

        private static string SubIndent(Match m)
        {
            string TmpStr = m.ToString();
            if (TmpStr.Contains(OneDataHelper.IndentSymbol))
            {
                TmpStr = TmpStr.Substring(OneDataHelper.IndentSymbol.Length);
            }
            return TmpStr;
        }

        private static string AddIndent(Match m)
        {
            return m.ToString() + OneDataHelper.IndentSymbol;
        }

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
