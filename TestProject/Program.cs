using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
