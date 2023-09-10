using Helper;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            TestOnePage();
            //TestFunc();


            Console.ReadKey();
        }




        static void TestOnePage()
        {
            // 读取本地的Xml文件
            XDocument XmlDoc = new XDocument();
            string XmlFilePath = @"D:\PageContent.xml";
            XmlDoc = XDocument.Load(XmlFilePath);

            OnePage TmpOnePage = new OnePage(XmlDoc);
            string PageContent = TmpOnePage.ToCSV();
            Console.WriteLine(PageContent);

            string FilePath = @"D:\PageContent.csv";
            File.WriteAllText(FilePath, PageContent, Encoding.UTF8);


        }

        static void TestFunc()
        {
            string Input = "第一行T第二行TT第三行TT第三行";
            string Pattern = "(T)+";
            string Replacement = @"$1--";

            Input =  Regex.Replace(Input, Pattern, Replacement, RegexOptions.Multiline);
            Console.WriteLine(Input);
        }

        public string ReplaceCC(Match m)
        // Replace each Regex cc match with the number of the occurrence.
        {
            return m.ToString() + "==";
        }


        public static string TransformString(string input)
        {
            string pattern = @"T+";

            Regex r = new Regex(pattern);
            Program p = new Program();
            MatchEvaluator myEvaluator = new MatchEvaluator(p.ReplaceCC);
            input = r.Replace(input, myEvaluator);


            // Use a regular expression to find and replace instances of "T" followed by one or more "T" characters
            //string replacement = "TT";
            //string result = Regex.Replace(input, pattern, replacement);

            //Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //if (regex.IsMatch(input))
            //{
            //    MatchCollection matchCollection = regex.Matches(input);
            //    foreach (Match match in matchCollection)
            //    {
            //        Console.WriteLine(match.ToString());
            //        input.Replace(match.ToString(), match.ToString() + "T");
            //    }
            //}
            Console.WriteLine(input);


            return "";
        }






        void TestFunction()
        {
            // 读取本地的Xml文件
            XDocument XmlDoc = new XDocument();
            string XmlFilePath = @"D:\PageContent.xml";
            XmlDoc = XDocument.Load(XmlFilePath);

            OnePage TmpOnePage = new OnePage(XmlDoc);
            string TmpStr = TmpOnePage.ToCSV();

            //string TestStr = "Hello";
            //TestStr += Environment.NewLine;
            //TestStr += "World";

            Console.WriteLine(TmpStr);
            Console.ReadKey();
        }



        void WriteXML()
        {
            // 1、创建Dom对象
            XDocument xDoc = new XDocument();
            XDeclaration xDec = new XDeclaration("1.0", "utf-8", null);
            xDoc.Declaration = xDec;

            //2.创建根节点
            XElement rootElement = new XElement("List");
            xDoc.Add(rootElement);

            //3.循环工ist集合创建Person节点
            for (int i = 0; i < 4; i++)
            {
                //为每个Person对象创建一个Person元素
                XElement xElementPerson = new XElement("Person");
                xElementPerson.SetAttributeValue("id", (i + 1).ToString());
                xElementPerson.SetElementValue("Name", "Name" + (i + 1).ToString());
                rootElement.Add(xElementPerson);
            }

            //4.保存到文件
            xDoc.Save("ListNex.xml");
        }

        void ReadXml()
        {
            //1.读取xm1文件(XDocument)
            //1.加载xm1文件
            XDocument document = XDocument.Load("rss sportslg.xml");
            //2,先获取根节点
            XElement rootElement = document.Root;
            //3.将xml的根元素加载到Treeview的根节点上
            TreeView treeview1 = new TreeView();
            TreeNode rootNode = treeview1.Nodes.Add(rootElement.Name.ToString());
            //4.递归加载
            LoadXmlRecur(rootElement, rootNode.Nodes);

        }

        private void LoadXmlRecur(XElement rootElement, TreeNodeCollection treeNodeCollection)
        {
            //获取根元素rootElement下的所有的子元素
            //rootElement.Elements ()
            //遍历rootElement下的所有的子元素（直接子元素）】
            foreach (XElement item in rootElement.Elements())
            {
                if (item.Elements().Count() == 0)
                {
                    treeNodeCollection.Add(item.Value);
                }
                else
                {
                    //将当前子元素加载到Treeview的节点集合中
                    TreeNode node = treeNodeCollection.Add(item.Name.ToString());
                    LoadXmlRecur(item, node.Nodes);
                }
            }
        }
    }
}
