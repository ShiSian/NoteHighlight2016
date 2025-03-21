﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using System.Xml.Linq;
using Extensibility;
using Microsoft.Office.Core;
using NoteHighlightAddin.Utilities;
using Application = Microsoft.Office.Interop.OneNote.Application;  // Conflicts with System.Windows.Forms
using System.Reflection;
using System.Drawing;
using Microsoft.Office.Interop.OneNote;
using System.Text;
using System.Linq;
using System.Threading;
using System.Web;
using GenerateHighlightContent;
using System.Globalization;
using Helper;

#pragma warning disable CS3003 // Type is not CLS-compliant

namespace NoteHighlightAddin
{
    [ComVisible(true)]
	[Guid("4C6B0362-F139-417F-9661-3663C268B9E9"), ProgId("NoteHighlight2016.AddIn")]

	public class AddIn : IDTExtensibility2, IRibbonExtensibility
	{

        #region /*****【接口实现】*****/
        /// <summary>
        /// IRibbonExtensibility接口
        /// 一个用于自定义 Microsoft Office 应用程序的 Ribbon 用户界面 (UI) 的接口。
        /// 这个接口提供了一种机制，允许开发者定义和修改 Ribbon 的元素，
        /// 如标签、按钮、下拉菜单等。它是在为 Office 创建 Add-ins 或扩展时经常使用的接口。
        /// </summary>
        // GetCustomUI：当 Office 应用程序需要加载 Ribbon 的自定义 UI 时，它会调用此方法。这个方法返回一个包含 Ribbon 定义的 XML 字符串。
        public string GetCustomUI(string RibbonID)
        {
            try
            {
                var workingDirectory = Path.Combine(ProcessHelper.GetDirectoryFromPath(Assembly.GetCallingAssembly().Location), "ribbon.xml");
                string file = File.ReadAllText(workingDirectory);
                return file;
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception from Addin.LoadRibbon:" + e.Message);
                return "";
            }
        }

        /// <summary>
        /// 【IDTExtensibility2接口】
        ///  一个 COM 接口，主要用于创建 Visual Studio 的 Add-ins 或其他 Microsoft Office 应用程序（如 Excel、Word、Outlook 等）的扩展。
        /// 这个接口提供了一组事件，允许开发者在 Add-in 的生命周期的不同阶段执行特定的操作，如加载、卸载、启动和关闭等。
        /// </summary>
        // OnConnection：当 Add-in 被加载到宿主应用程序中时触发。例如，当用户启动应用程序或手动加载 Add-in 时。
        [CLSCompliant(false)]
        public void OnConnection(object Application, ext_ConnectMode ConnectMode, object AddInInst, ref Array custom)
        {
            OneNoteApplication = (Application)Application;
            OneDataHelper.OneNoteApplication = OneNoteApplication;
        }
        // OnDisconnection:当 Add-in 从宿主应用程序中卸载时触发。这可以是用户手动卸载 Add-in，或是当应用程序关闭时。
        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect")]
        [CLSCompliant(false)]
        public void OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom)
        {
            OneNoteApplication = null;
            OneDataHelper.OneNoteApplication = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        // OnAddInsUpdate:当 Add-ins 的集合发生更改时触发。
        public void OnAddInsUpdate(ref Array custom)
        {
        }
        // OnStartupComplete:在宿主应用程序完成启动并加载所有 Add-ins 后触发。
        public void OnStartupComplete(ref Array custom)
        {
        }
        // OnBeginShutdown:在开始关闭宿主应用程序前触发。
        public void OnBeginShutdown(ref Array custom)
        {
            this.mainForm?.Invoke(new Action(() =>
            {
                // close the form on the forms thread
                this.mainForm?.Close();
                this.mainForm = null;
            }));
        }
        #endregion




        // OneNote程序
        protected Application OneNoteApplication{ get; set; }
        public XNamespace ns;
        // 代码窗口
        private MainForm mainForm;
        string tag;
        private bool QuickStyle { get; set; }
        private bool DarkMode { get; set; }





    #region /*****【函数部分】*****/
        // 获取当前激活的OneNote页面ID
        private string GetActivedPageID()
        {
            Windows OneNoteWindows = OneNoteApplication.Windows;
            Window OneNoteActiveWindow = OneNoteWindows.CurrentWindow;
            return OneNoteActiveWindow.CurrentPageId;
        }

        // 获取鼠标位置
        private string[] GetMousePointPosition(string pageXml)
        {
            var node = XDocument.Parse(pageXml).Descendants(ns + "Outline")
                                               .Where(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "partial")
                                               .FirstOrDefault();
            if (node != null)
            {
                var attrPos = node.Descendants(ns + "Position").FirstOrDefault();
                if (attrPos != null)
                {
                    var x = attrPos.Attribute("x").Value;
                    var y = attrPos.Attribute("y").Value;
                    return new string[] { x, y };
                }
            }
            return null;
        }
        #endregion

        // 获取当前激活页面的内容
        [CLSCompliant(false)]
        public OnePage GetCurrentPage()
        {
            return null;
        }

        // 导出XML
        [CLSCompliant(false)]
        public void ExportXML(IRibbonControl control)
        {
            string CurrentPageID = GetActivedPageID();
            string PageContent;
            OneNoteApplication.GetPageContent(CurrentPageID, out PageContent);

            if (!string.IsNullOrEmpty(PageContent))
            {

                string FilePath = @"D:\PageContent.xml";
                OneDataHelper.SavePage2XML(PageContent, FilePath);

                //OneDataHelper.SavePage2CSV(PageContent, FilePath);

                //File.WriteAllText(FilePath, PageContent, Encoding.UTF8);
                MessageBox.Show("Succeed save xml file.");
            }
            else
            {
                MessageBox.Show("Failed to save xml file.");
            }
        }

        // 导出CSV
        [CLSCompliant(false)]
        public void ExportCSV(IRibbonControl control)
        {
            string CurrentPageID = GetActivedPageID();
            string PageContent;
            OneNoteApplication.GetPageContent(CurrentPageID, out PageContent);

            if (!string.IsNullOrEmpty(PageContent))
            {

                string FilePath = @"D:\PageContent.csv";
                OneDataHelper.SavePage2CSV(PageContent, FilePath);
                MessageBox.Show("Succeed save csv file.");
            }
            else
            {
                MessageBox.Show("Failed to save csv file.");
            }
        }

        // 导出Html
        [CLSCompliant(false)]
        public void ExportHtml(IRibbonControl control)
        {
            string CurrentPageID = GetActivedPageID();
            string PageContent;
            OneNoteApplication.GetPageContent(CurrentPageID, out PageContent);

            if (!string.IsNullOrEmpty(PageContent))
            {

                string FilePath = @"D:\PageContent.html";
                OneDataHelper.SavePage2Html(PageContent, FilePath);
                MessageBox.Show("Succeed save Html file.");
            }
            else
            {
                MessageBox.Show("Failed to save Html file.");
            }
        }

        // 导出表格
        [CLSCompliant(false)]
        public void ExportTable(IRibbonControl control)
        {
            string CurrentPageID = GetActivedPageID();
            string PageContent;
            OneNoteApplication.GetPageContent(CurrentPageID, out PageContent);

            if (!string.IsNullOrEmpty(PageContent))
            {
                string FilePath = @"D:\PageContent.html";
                OneDataHelper.SavePageTable(PageContent, FilePath);
                MessageBox.Show("Succeed to save table content.");
            }
            else
            {
                MessageBox.Show("Failed to save table content.");
            }
        }

        public AddIn()
		{
		}



        [CLSCompliant(false)]
        public bool cbQuickStyle_GetPressed(IRibbonControl control)
        {
            this.QuickStyle = NoteHighlightForm.Properties.Settings.Default.QuickStyle;
            return this.QuickStyle;
        }

        [CLSCompliant(false)]
        public void cbQuickStyle_OnAction(IRibbonControl control, bool isPressed)
        {
            this.QuickStyle = isPressed;
            NoteHighlightForm.Properties.Settings.Default.QuickStyle = this.QuickStyle;
            NoteHighlightForm.Properties.Settings.Default.Save();
        }

        [CLSCompliant(false)]
        public bool cbDarkMode_GetPressed(IRibbonControl control)
        {
            this.DarkMode = NoteHighlightForm.Properties.Settings.Default.DarkMode;
            return this.DarkMode;
        }

        [CLSCompliant(false)]
        public void cbDarkMOde_OnAction(IRibbonControl control, bool isPressed)
        {
            this.DarkMode = isPressed;
            NoteHighlightForm.Properties.Settings.Default.DarkMode = this.DarkMode;
            NoteHighlightForm.Properties.Settings.Default.Save();
        }

        //public async Task AddInButtonClicked(IRibbonControl control)
        [CLSCompliant(false)]
        public void AddInButtonClicked(IRibbonControl control)
        {
            try
            {
                tag = control.Tag;

                Thread t = new Thread(new ThreadStart(ShowForm));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception from AddInButtonClicked: "+ e.ToString());
            }
        }

        private void ShowForm()
        {
            try
            {
                string outFileName = Guid.NewGuid().ToString();
                var pageNode = GetPageNode();
                string pageXml = GetPageXml(pageNode.Attribute("ID").Value);
                string selectedText = "";
                XElement outline = null;
                bool selectedTextFormated = false;

                if (pageNode != null)
                {
                    selectedText = GetSelectedText(pageXml, out selectedTextFormated);

                    if (selectedText.Trim() != "")
                    {
                        outline = GetOutline(pageXml);
                    }
                }

                MainForm form = new MainForm(tag, outFileName, selectedText, this.QuickStyle, this.DarkMode);

                System.Windows.Forms.Application.Run(form);
                string fileName = Path.Combine(Path.GetTempPath(), outFileName + ".html");

                if (File.Exists(fileName))
                {
                    InsertHighLightCodeToCurrentSide(fileName, pageXml, form.Parameters, outline, selectedTextFormated);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception from ShowForm: " + e.ToString());
            }
        }





        [CLSCompliant(false)]
        public void SettingsButtonClicked(IRibbonControl control)
        {
            try
            {
               
                Thread t = new Thread(new ThreadStart(ShowSettingsForm));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception from SettingsButtonClicked: " + e.ToString());
            }
        }

        private void ShowSettingsForm()
        {
            try
            {
             
                SettingsForm form = new SettingsForm();

                System.Windows.Forms.Application.Run(form);
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception from ShowForm: " + e.ToString());
            }
        }

        /// <summary>
        /// Specified in Ribbon.xml, this method returns the image to display on the ribbon button
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public IStream GetImage(string imageName)
		{
			MemoryStream imageStream = new MemoryStream();

            BindingFlags flags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var b = typeof(Properties.Resources).GetProperty(imageName.Substring(0, imageName.IndexOf('.')), flags).GetValue(null, null) as Bitmap;
            b.Save(imageStream, ImageFormat.Png);

            return new CCOMStreamWrapper(imageStream);
		}

        // 插入生成好的代码到光标所在位置
        private void InsertHighLightCodeToCurrentSide(string fileName, string pageXml, HighLightParameter parameters, XElement outline, bool selectedTextFormated)
        {
            try
            {
                // Trace.TraceInformation(System.Reflection.MethodBase.GetCurrentMethod().Name);
                string htmlContent = File.ReadAllText(fileName, new UTF8Encoding(false));

                string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                htmlContent = htmlContent.Replace(byteOrderMarkUtf8, "");

                var pageNode = GetPageNode();

                if (pageNode != null)
                {
                    var existingPageId = pageNode.Attribute("ID").Value;
                    string[] position = null;
                    if (outline == null)
                    {
                        position = GetMousePointPosition(pageXml);
                    }

                    var page = InsertHighLightCode(htmlContent, position, parameters, outline, (new GenerateHighLight()).Config, selectedTextFormated, IsSelectedTextInline(pageXml));
                    page.Root.SetAttributeValue("ID", existingPageId);

                    //Bug fix - remove overflow value for Indents
                    foreach (var el in page.Descendants(ns + "Indent").Where(n => double.Parse(n.Attribute("indent").Value, new CultureInfo(page.Root.Attribute("lang").Value)) > 1000000))
                    {
                        el.Attribute("indent").Value = "0";
                    }

                    OneNoteApplication.UpdatePageContent(page.ToString(), DateTime.MinValue);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception from InsertHighLightCodeToCurrentSide: "+e.ToString());
            }
        }



        XElement GetPageNode()
        {
            string notebookXml;
            try
            {
                OneNoteApplication.GetHierarchy(null, HierarchyScope.hsPages, out notebookXml, XMLSchema.xs2013);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception from onApp.GetHierarchy:" + ex.Message);
                return null; ;
            }

            var doc = XDocument.Parse(notebookXml);
            ns = doc.Root.Name.Namespace;

            var pageNode = doc.Descendants(ns + "Page")
                              .Where(n => n.Attribute("isCurrentlyViewed") != null && n.Attribute("isCurrentlyViewed").Value == "true")
                              .FirstOrDefault();
            return pageNode;
        }



        private XElement GetOutline(string pageXml)
        {
            var node = XDocument.Parse(pageXml).Descendants(ns + "Outline")
                                               .Where(n => n.Attribute("selected") != null && (n.Attribute("selected").Value == "all" || n.Attribute("selected").Value == "partial"))
                                               .FirstOrDefault();

            return node;
        }



        private string GetPageXml(string pageID)
        {
            string pageXml;
            OneNoteApplication.GetPageContent(pageID, out pageXml, PageInfo.piSelection);

            return pageXml;
        }

        public string GetSelectedText(string pageXml, out bool selectedTextFormated)
        {
            var node = XDocument.Parse(pageXml).Descendants(ns + "Outline")
                                               .Where(n => n.Attribute("selected") != null && (n.Attribute("selected").Value == "all" || n.Attribute("selected").Value == "partial"))
                                               .FirstOrDefault();
            
            StringBuilder sb = new StringBuilder();
            selectedTextFormated = false;
            if (node != null)
            {
                var table = node.Descendants(ns + "Table").Where(n => n.Attribute("selected") != null && (n.Attribute("selected").Value == "all" || n.Attribute("selected").Value == "partial")).FirstOrDefault();

                System.Collections.Generic.IEnumerable<XElement> attrPos;
                if (table == null)
                {
                    attrPos = node.Descendants(ns + "T").Where(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "all");
                }
                else
                {
                    attrPos = table.Descendants(ns + "Cell").LastOrDefault().Descendants(ns + "T").Where(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "all");
                    selectedTextFormated = true;
                }
                int tabCount = 0;
                int initTabCount = -1;
                foreach (var line in attrPos)
                {
                    var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                    htmlDocument.LoadHtml(line.Value);

                    if (initTabCount == -1)
                    {
                        initTabCount = line.Ancestors().Elements(ns + "T").Count();
                    }
                    tabCount = line.Ancestors().Elements(ns + "T").Count() - initTabCount;


                    sb.AppendLine(new String('\t', tabCount) + HttpUtility.HtmlDecode(htmlDocument.DocumentNode.InnerText));
                }
            }
            return sb.ToString().TrimEnd('\r','\n');
        }

        [CLSCompliant(false)]
        public bool IsSelectedTextInline(string pageXml)
        {
            var node = XDocument.Parse(pageXml).Descendants(ns + "Outline")
                                               .Where(n => n.Attribute("selected") != null && (n.Attribute("selected").Value == "all" || n.Attribute("selected").Value == "partial"))
                                               .FirstOrDefault();

            if (node != null)
            {
                var table = node.Descendants(ns + "Table").Where(n => n.Attribute("selected") != null && (n.Attribute("selected").Value == "all" || n.Attribute("selected").Value == "partial")).FirstOrDefault();

                System.Collections.Generic.IEnumerable<XElement> attrPos;
                if (table == null)
                {
                    foreach (var oeNode in node.Descendants(ns + "OE"))
                    {
                        if (oeNode.Descendants(ns + "T").Where(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "all").Count() > 0
                                        && oeNode.Descendants(ns + "T").Where(n => n.Attribute("selected") == null || n.Attribute("selected").Value == "none").Count() > 0)
                        {
                            return true;
                        } 
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 產生 XML 插入至 OneNote
        /// Generate XML Insert To OneNote
        /// </summary>
        [CLSCompliant(false)]
        public XDocument InsertHighLightCode(string htmlContent, string[] position, HighLightParameter parameters, XElement outline, HighLightSection config, bool selectedTextFormated, bool isInline)
        {
            XElement children = PrepareFormatedContent(htmlContent, parameters, config, isInline);

            bool update = false;
            if (outline == null)
            {
                outline = CreateOutline(position, children);
            }
            else // Update exiting outline
            {
                update = true;

                //Change outline width
                outline.Element(ns + "Size").Attribute("width").Value = "1600";

                if (selectedTextFormated)
                {
                    outline.Descendants(ns + "Table").Where(n => n.Attribute("selected") != null &&
                                        (n.Attribute("selected").Value == "all" || n.Attribute("selected").Value == "partial")).FirstOrDefault().ReplaceWith(children.Descendants(ns + "Table").FirstOrDefault());
                    //outline.Descendants().Where(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "all").Remove();
                }
                else
                {
                    if (isInline)
                    {
                        int j = 0;
                        for(int i = 0; i < outline.Descendants(ns + "OE").Count(); i++)
                        {
                            XElement oeNode = outline.Descendants(ns + "OE").ElementAt(i);

                            if (oeNode.Descendants(ns + "T").Where(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "all").Count() > 0)
                            {
                                oeNode.Descendants(ns + "T").Where(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "all").FirstOrDefault().ReplaceWith(children.Descendants(ns + "Table").Descendants(ns + "OEChildren").Descendants(ns + "OE").ElementAt(j).Descendants(ns + "T"));
                                j++;
                            }

                        }
                        outline.Descendants(ns + "OE").Where(t => t.Elements(ns + "T").Any(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "all")).Remove();
                    }
                    else
                    {
                        outline.Descendants(ns + "T").Where(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "all").FirstOrDefault().ReplaceWith(children.Descendants(ns + "Table").FirstOrDefault());
                        outline.Descendants(ns + "OE").Where(t => t.Elements(ns + "T").Any(n => n.Attribute("selected") != null && n.Attribute("selected").Value == "all")).Remove();
                        outline.Descendants(ns + "OEChildren").Where(n => n.HasElements == false && n.Attribute("selected") != null && (n.Attribute("selected").Value == "partial")).Remove();
                    }
                }
            }

            if (update)
            {
                return outline.Parent.Document;
            }
            else
            {
                XElement page = new XElement(ns + "Page");
                page.Add(outline);

                XDocument doc = new XDocument();
                doc.Add(page);
                return doc;
            }


        }

        private XElement CreateOutline(string[] position, XElement children)
        {
            XElement outline = new XElement(ns + "Outline");
            if (position != null && position.Length == 2)
            {
                XElement pos = new XElement(ns + "Position");
                pos.Add(new XAttribute("x", position[0]));
                pos.Add(new XAttribute("y", position[1]));
                outline.Add(pos);

                XElement size = new XElement(ns + "Size");
                size.Add(new XAttribute("width", "1600"));
                size.Add(new XAttribute("height", "200"));
                outline.Add(size);
            }
            outline.Add(children);
            return outline;
        }

        private XElement PrepareFormatedContent(string htmlContent, HighLightParameter parameters, HighLightSection config, bool isInline)
        {
            XElement children = new XElement(ns + "OEChildren");

            XElement table = new XElement(ns + "Table");
            table.Add(new XAttribute("bordersVisible", NoteHighlightForm.Properties.Settings.Default.ShowTableBorder));

            XElement columns = new XElement(ns + "Columns");
            XElement column1 = new XElement(ns + "Column");
            column1.Add(new XAttribute("index", "0"));
            column1.Add(new XAttribute("width", "40"));
            if (parameters.ShowLineNumber && !isInline)
            {
                columns.Add(column1);
            }
            XElement column2 = new XElement(ns + "Column");
            if (parameters.ShowLineNumber && !isInline)
            {
                column2.Add(new XAttribute("index", "1"));
            }
            else
            {
                column2.Add(new XAttribute("index", "0"));
            }

            column2.Add(new XAttribute("width", "1400"));
            columns.Add(column2);

            table.Add(columns);

            Color color = parameters.HighlightColor;
            string colorString = color.A == 0 ? "none" : string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);

            XElement row = new XElement(ns + "Row");
            XElement cell1 = new XElement(ns + "Cell");
            cell1.Add(new XAttribute("shadingColor", colorString));
            XElement cell2 = new XElement(ns + "Cell");
            cell2.Add(new XAttribute("shadingColor", colorString));


            string defaultStyle = "";

            var arrayLine = htmlContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var it in arrayLine)
            {
                string item = it;

                if (item.StartsWith("<pre"))
                {
                    defaultStyle = item.Substring(0, item.IndexOf(">") + 1);
                    //Sets language to Latin to disable spell check
                    defaultStyle = defaultStyle.Insert(defaultStyle.Length - 1, " lang=la");

                    if (this.DarkMode)
                    {
                        //Remove background-color element so that text would render with correct contrast in dark mode
                        int bcIndex = defaultStyle.IndexOf("background-color");
                        defaultStyle = defaultStyle.Remove(bcIndex, defaultStyle.IndexOf(';', bcIndex) - bcIndex +1);
                    }

                    item = item.Substring(item.IndexOf(">") + 1);
                }

                if (item == "</pre>")
                {
                    continue;
                }

                var itemNr = "";
                var itemLine = "";
                if (parameters.ShowLineNumber)
                {
                    if (item.Contains("</span>"))
                    {
                        int ind = item.IndexOf("</span>");
                        itemNr = item.Substring(0, ind + ("</span>").Length);
                        itemLine = item.Substring(ind);
                    }
                    else
                    {
                        itemNr = "";
                        itemLine = item;
                    }

                    //string nr = string.Format(@"<body style=""font-family:{0}"">", GenerateHighlightContent.GenerateHighLight.Config.OutputArguments["Font"].Value) +
                    //        itemNr.Replace("&apos;", "'") + "</body>";
                    string nr = "";
                    if (string.IsNullOrEmpty(config.LineNrReplaceCh))
                    {
                        nr = defaultStyle + itemNr.Replace("&apos;", "'") + "</pre>";
                    }
                    else
                    {
                        nr = defaultStyle + config.LineNrReplaceCh.PadLeft(5) + "</pre>";
                    }

                    XElement oeElement = new XElement(ns + "OE",
                                    new XElement(ns + "T",
                                        new XCData(nr)));
                    if (ContainsAsianCharacter(itemLine))
                    {
                        oeElement.Add(new XAttribute("spaceBefore", config.AsianBeforeSpace));
                        oeElement.Add(new XAttribute("spaceAfter", config.AsianAfterSpace));
                    }

                    cell1.Add(new XElement(ns + "OEChildren",
                               oeElement ));
                }
                else
                {
                    itemLine = item;
                }
                //string s = item.Replace(@"style=""", string.Format(@"style=""font-family:{0}; ", GenerateHighlightContent.GenerateHighLight.Config.OutputArguments["Font"].Value));
                //string s = string.Format(@"<body style=""font-family:{0}"">", GenerateHighlightContent.GenerateHighLight.Config.OutputArguments["Font"].Value) + 
                //            itemLine.Replace("&apos;", "'") + "</body>";
                string s = defaultStyle + itemLine.Replace("&apos;", "'") + "</pre>";

                cell2.Add(new XElement(ns + "OEChildren",
                            new XElement(ns + "OE",
                                new XElement(ns + "T",
                                    new XCData(s)))));

            }

            if (parameters.ShowLineNumber && !isInline)
            {
                row.Add(cell1);
            }
            row.Add(cell2);

            table.Add(row);

            children.Add(new XElement(ns + "OE",
                                table));
            return children;
        }

        private bool ContainsAsianCharacter(string itemLine)
        {
            return itemLine.Any(c => (uint)c >= 0x4E00 && (uint)c <= 0x2FA1F);
        }
    }
}
