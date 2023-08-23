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
            string xmlString = @"
            <one:PageSettings RTL="false" color="automatic">
    < one:PageSize >
 
       < one:Automatic />
  
      </ one:PageSize >
   
       < one:RuleLines visible = "false" />
    
      </ one:PageSettings >
                 ";

            XElement rootElement = XElement.Parse(xmlString);

            // 1. 获取子元素的值
            string childValue = rootElement.Element("ChildElement").Value;
            Console.WriteLine($"ChildElement Value: {childValue}");

            // 2. 获取属性的值
            string attributeValue = rootElement.Element("AnotherChild").Attribute("attributeKey").Value;
            Console.WriteLine($"Attribute Value: {attributeValue}");

            Console.ReadLine();
        }
    }
}
