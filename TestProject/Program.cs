using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // 读取本地的Xml文件
            XDocument XmlDoc = new XDocument();
            string XmlFilePath = @"D:\PageContent.xml";
            XmlDoc = XDocument.Load(XmlFilePath);

            var Title = XmlDoc.Descendants("Title");
            if (Title != null)
            {
                //string Tmp = Title.element
            }

            Console.WriteLine(Title.ToString());



            Console.ReadLine();
        }

        void WriteXML()
        {
            // 1、创建Dom对象
            XDocument xDoc = new XDocument();
            XDeclaration xDec = new XDeclaration("1.0", "utf-8", null);
            xDoc.Declaration = xDec;

            //2.创建根节点
            XElement rootElement = new XElement ("List"); 
            xDoc.Add (rootElement);

            //3.循环工ist集合创建Person节点
            for (int i = 0; i < 4; i++)
            { 
                //为每个Person对象创建一个Person元素
                XElement xElementPerson = new XElement("Person");
                xElementPerson.SetAttributeValue("id", (i + 1).ToString());
                xElementPerson.SetElementValue("Name", "Name"+ (i + 1).ToString());
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
            TreeNode rootNode = treeview1.Nodes.Add(rootElement.Name.Tostring())
        }
    }
}
