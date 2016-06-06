using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Markup;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Norne_Beta.UIElements;
using PythonLib;

namespace Norne_Beta
{

    class LayoutRow
    {
        public List<string> Element { get; set; }
        public List<string> Data { get; set; }
        public List<string> VizContainer { get; set; }
        public LayoutRow()
        {
            Element = new List<string>();
            Data = new List<string>();
            VizContainer = new List<string>();
        }
    }

    class TemplateHelper
    {
        public static string GetName(HorizontalTemplate t)
        {
            string tName = t.GetType().Name;
            return tName;
        }

        public static List<LayoutRow> GetLayoutMatrix(HorizontalTemplate t)
        {
            List<LayoutRow> matrix = new List<LayoutRow>();

            foreach (var ele in t.MainPanel.Children)
            {
                string tName = ele.GetType().Name;

                if(tName == TemplateName.DockPanel)
                {
                }
                else
                {
                    LayoutRow lRow = new LayoutRow();
                    lRow.Element.Add(tName);
                    matrix.Add(lRow);
                }
            }
            return matrix;
        }
    }

    
    class XTemplate 
    {
        public string name;

        public XDocument xml;
        public XElement rootGrid;
        public XNamespace ns;

        public XTemplate(Object rootTemplate)
        {
            xml = InitialteXML(rootTemplate);
            ns = xml.Root.LastAttribute.Value;
            rootGrid = xml.Root.Element(ns + "Grid");
        }

        private XDocument InitialteXML(Object rt)
        {
            string s = XamlWriter.Save(rt);
            StringReader sr = new StringReader(s);
            XDocument xml = XDocument.Load(sr);
            return xml;
        }


        public IEnumerable<XElement> GetChildren(string ElementName)
        {
            IEnumerable<XElement> children = from el in rootGrid.Elements(ns + ElementName)
                                             select el;
            return children;
        }

        public XElement GetMainPanel()
        {
            var elements = from el in rootGrid.Elements(ns + "DockPanel")
                   where (string)el.Attribute("Name") == "MainPanel"
                   select el;
            foreach (XElement e in elements)
                return e;

            return null;
        }

        public string GetName()
        {
            return xml.Root.Name.LocalName;
        }

    }

    class TemplateGenerator<T> where T :HorizontalTemplate
    {
        private StreamWriter file;
        private T t;
        private PyWriter py;

        public TemplateGenerator(T ht)
        {
            file = File.CreateText(ht.FilePath);
            t = ht;
            py = new PyWriter(file, t);
        }

        public void WriteTemplate()
        {
            WriteHeader();
            WriteImports();
            WriteInit();
            WriteUI();
            WriteController();
            WriteStatemachine();
            file.Close();
        }

        public void WriteHeader()
        {
        }


        public void WriteImports()
        {
            py.Import();
        }

        public void WriteInit()
        {
            py.InitUI();

        }

        public void WriteUI()
        {
            py.SetupController();
            py.SetupStateMachine();
            py.CreateUI();
        }

        public void WriteController()
        {
            py.WriteStateMachine();
        }

        public void WriteStatemachine()
        {
        }
    }
}
