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






        /// <summary>
        /// 绑定数据
        /// </summary>
        public void SetBind()
        {
            List<Student> list = GetData(20);
            CreateFile(@"c:/students.xml", "students", list);
            AddXmlElement(@"c:/students.xml");
            //UpdateXmlElement(@"c:/students.xml",""+104);
            // RemoveXmlElement(@"c:/students.xml");
            //ConvertXmlAttributeToElement(@"c:/students.xml");
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<Student> GetData(int n)
        {
            List<Student> list = new List<Student>();
            Student s;
            for (int i = 0; i < n; i++)
            {

                if (i % 2 == 0)
                {
                    s = new Student(i * 10 + 40, "nihao" + i, "男");
                }
                else
                {
                    s = new Student(i * 10 + 40, "nihao" + i, "女");
                }
                list.Add(s);
            }

            return list;

        }

        /// <summary>
        /// 使用linq 建立xml
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="rootElement">根元素</param>
        /// <param name="list">对象</param>
        public void CreateFile(string filePath, string rootElement, List<Student> list)
        {
            if (File.Exists(filePath)) //文件存在就删除
            {
                FileInfo file = new FileInfo(filePath);
                file.Delete();
            }
            XElement contacts = new XElement(rootElement);
            foreach (Student s in list)
            {
                XElement el = new XElement("student", new XAttribute("number", s.Number),
                    new XElement("name", s.Name),
                    new XElement("sex", s.Sex)
                    );

                contacts.Add(el);
            }
            contacts.Save(filePath);
        }

        /// <summary>
        /// //增加元素到XML文件
        /// </summary>
        /// <param name="filename">文件名</param>
        private void AddXmlElement(string filename)
        {
            ///导入XML文件
            XElement xe = XElement.Load(filename);
            ///创建一个新的节点
            XElement student = new XElement("student",
             new XAttribute("number", "104"),                    ///添加属性number
             new XElement("name", "chenxt"),                     ///添加元素Name
             new XElement("sex", "男")                           ///添加元素sex
             );
            ///添加节点到文件中，并保存
            xe.Add(student);
            xe.Save(filename);
            ///显示XML文件的内容
            Response.Write(xe);
            ///设置网页显示的形式为XML文件
            Response.ContentType = "text/xml";
            Response.End();
        }

        /// <summary>
        /// //修改XML文件中的元素
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="number">number</param>
        private void UpdateXmlElement(string filePath, string number)
        { ///导入XML文件

            XElement xe = XElement.Load(filePath);
            ///查找被替换的元素
            IEnumerable<XElement> element = from e in xe.Elements("student")
                                            where e.Attribute("number").Value == "104"
                                            select e;


            //IEnumerable<XElement> element = from e in xe.Elements("student")
            //                                where e.Element("sex").Value=="女"
            //                                select e;

            // XElement x= element.ElementAt(0);

            ///替换为新元素，并保存
            if (element.Count() > 0)
            {
                XElement first = element.First();
                ///设置新的属性
                first.SetAttributeValue("number", "106");
                ///替换新的节点
                first.ReplaceNodes(
                 new XElement("Name", "陈宪涛"),              ///添加元素Name
                 new XElement("sex", "男")                   ///添加元素Price
                 );
            }
            xe.Save(filePath);
            ///显示XML文件的内容
            Response.Write(xe);
            ///设置网页显示的形式为XML文件
            Response.ContentType = "text/xml";
            Response.End();
        }
        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="filePath"></param>
        private void RemoveXmlElement(string filePath)//删除XML文件中的元素
        {
            ///导入XML文件
            XElement xe = XElement.Load(filePath);
            ///查找被删除的元素
            IEnumerable<XElement> element = from e in xe.Elements()
                                            where (string)e.Element("name") == "nihao1"
                                            select e;
            ///删除指定的元素，并保存
            if (element.Count() > 0)
            {
                element.First().Remove();
            }
            xe.Save(filePath);
            ///显示XML文件的内容
            Response.Write(xe);
            ///设置网页显示的形式为XML文件
            Response.ContentType = "text/xml";
            Response.End();
        }
        /// <summary>
        /// 将属性名改为节点名
        /// </summary>
        /// <param name="filePath">路径名</param>
        private void ConvertXmlAttributeToElement(string filePath)//将XML文件中的属性转换为元素
        { ///导入XML文件

            XElement xe = XElement.Load(filePath);
            ///查找被替换的元素
            IEnumerable<XElement> element = from e in xe.Elements("student")
                                            where e.Attribute("number").Value == "106"
                                            select e;
            ///替换为新元素，并保存
            if (element.Count() > 0)
            {
                XElement first = element.First();
                ///获取第一个属性
                XAttribute attribute = first.FirstAttribute;
                ///将属性转换为元素
                first.AddFirst(
                 new XElement(attribute.Name, attribute.Value) ///添加元素ID
                 );
                ///删除属性
                first.RemoveAttributes();
            }
            xe.Save(filePath);
            ///显示XML文件的内容
            Response.Write(xe);
            ///设置网页显示的形式为XML文件
            Response.ContentType = "text/xml";
            Response.End();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filePath">路径</param>
        public void SearchXML(string filePath)
        {
            XElement xe = XElement.Load(filePath);
            //IEnumerable<XElement> items =
            //    from el in xe.Descendants("name")
            //    let name = (string)el.Value
            //    orderby name
            //    select el;
            //foreach (XElement prdName in items)
            //{
            //    Response.Write(prdName.Name + ":" + (string)prdName + "</br>");
            //}

            ///投影匿名类型
            IEnumerable<Student> items =
                from el in xe.Descendants("student")
                let name = (string)el.Element("name").Value
                orderby name
                select new Student
                {
                    Number = Convert.ToInt32(el.Attribute("number").Value),
                    Name = el.Element("name").Value,
                    Sex = el.Element("sex").Value
                };

            foreach (Student prdName in items)
            {
                Response.Write(prdName.Name + "</br>");

            }
        }
    }

    internal class Student
    {
    }
}
