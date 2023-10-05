using Helper.OneData;
using Microsoft.Office.Interop.OneNote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Application = Microsoft.Office.Interop.OneNote.Application;


namespace Helper
{
    public class OneDataHelper
    {
        public static XNamespace OneSpace;
        public static string IndentSymbol;
        public static Application OneNoteApplication;

        public OneDataHelper()
        {
            IndentSymbol = "iiii" + Guid.NewGuid().ToString() + "iiii";
        }

        public static string GetActivedPageID()
        {
            Windows OneNoteWindows = OneNoteApplication.Windows;
            Window OneNoteActiveWindow = OneNoteWindows.CurrentWindow;
            return OneNoteActiveWindow.CurrentPageId;
        }

        public static string GetImageBase64(string InCallbackID)
        {
            string ImageStr = "";
            if (OneNoteApplication != null)
            {
                OneNoteApplication.GetBinaryPageContent(GetActivedPageID(), InCallbackID, out ImageStr);
            }
            return ImageStr;
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
            return InStr.Replace(OneDataHelper.IndentSymbol, "____");
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

        // 导出Html
        public static void SavePage2Html(string InPageString, string InTargetFilePath)
        {
            XDocument Doc = XDocument.Parse(InPageString);
            if (Doc != null)
            {
                OnePage TmpPage = new OnePage(Doc);
                string OutHtml = TmpPage.ToHtml();

                using (StreamWriter TmpWriter = new StreamWriter(InTargetFilePath))
                {
                    TmpWriter.Write(OutHtml);
                }
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

        public static void AutoConstructItem( ref List<OneItem> OutOneItemList, XElement XElem, int InIndentNum)
        {
            XName TmpName = XElem.Name;

            if (TmpName == (OneSpace + "T"))
            {
                OneT TmpOneT = new OneT(XElem, InIndentNum);
                OutOneItemList.Add(TmpOneT);
            }
            else if (TmpName == (OneSpace + "List"))
            {
                OneList TmpOneList = new OneList(XElem, InIndentNum);
                OutOneItemList.Add(TmpOneList);
            }
            else if (TmpName == (OneSpace + "Tag"))
            {
                OneTag TmpOneTag = new OneTag(XElem);
                OutOneItemList.Add(TmpOneTag);
            }
            else if (TmpName == (OneSpace + "OEChildren"))
            {
                OneOEChildren TmpOneOEChildren = new OneOEChildren(XElem, InIndentNum);
                OutOneItemList.Add(TmpOneOEChildren);
            }
            else if (TmpName == (OneSpace + "Image"))
            {
                OneImage TmpOneImage = new OneImage(XElem, InIndentNum);
                OutOneItemList.Add(TmpOneImage);
            }
            else if (TmpName == (OneSpace + "Table"))
            {
                OneTable TmpOneTable = new OneTable(XElem, InIndentNum);
                OutOneItemList.Add(TmpOneTable);
            }
            else
            {

            }
        }
    }
}
