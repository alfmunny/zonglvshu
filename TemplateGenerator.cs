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

    class TemplateGenerator<T> where T :HorizontalTemplate
    {
        private StreamWriter file;
        private T t;
        private PyWriter py;
        private PyParser parser;

        public TemplateGenerator(T ht)
        {
            t = ht;
            parser = new PyParser(t.FilePath);
        }

        public void WriteImports()
        {
            file = File.AppendText(t.FilePath);
            py = new PyWriter(file, t);
            py.Import();
            file.Close();
        }

        public void WriteTemplate()
        {
            file = File.AppendText(t.FilePath);
            py = new PyWriter(file, t);
            WriteUI();
            WriteController();
            WriteStatemachine();
            file.Close();
        }

        public void WriteInit()
        {
        }

        public void WriteUI()
        {
            py.InitUI();
            py.SetupController();
            py.SetupStateMachine();
            py.CreateUI();
        }

        public void WriteHighlights()
        {
            py.WriteSetHighlights();
        }

        public void WriteController()
        {
        }

        public void WriteStatemachine()
        {
            py.WriteStateMachine();
        }

        public void UpdateTempalte()
        {
            int lineNumber = 1;
            string line = null;
            bool isUpdated = false;

            int[] uiNums = parser.GetClassLineNumber(t.UIClassName);
            int[] gfxNums = parser.GetClassLineNumber(t.GfxClassName);
            int begin = uiNums[0];
            // Plus 1 to include the space line
            int end = gfxNums[1] + 1;

            string fileBackup = t.FilePath + ".backup.py";
            using (StreamReader reader = new StreamReader(t.FilePath))
            {
                using (FileStream fs = File.Create(fileBackup))
                {
                }

                using (StreamWriter s = File.AppendText(fileBackup))
                {
                    py = new PyWriter(s, t);
                    while((line = reader.ReadLine()) != null)
                    {
                        if (lineNumber < begin || lineNumber > end)
                        {
                            s.WriteLine(line);
                        }
                        else if (! isUpdated)
                        {

                            WriteUI();
                            WriteController();
                            WriteStatemachine();
                            isUpdated = true;
                        }
                        lineNumber += 1;
                    }
                }
            }
            System.IO.File.Delete(t.FilePath);
            System.IO.File.Move(fileBackup, t.FilePath);
        }

        public void DeleteTemplate()
        {

        }

        public bool isTemplateExists()
        {
            return parser.GetTemplates().Contains(t.UIClassName);
        }
    }
}
