using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace PythonLib
{
    class PyParser
    {
        private string fp;
        private string pyLibFolder = @"""E:\Projects\microsoft\Norne Beta\Libs\Scripts""";
        public dynamic TemplateJson;
        
        public PyParser(string filePath)
        {
            this.fp = filePath;
            this.TemplateJson = null;
        }

        public string[] GetTemplates()
        {
            string script = pyLibFolder + @"\templates_finder.py";
            string output = GetPyStdOutput(script);
            string[] results = StringUtils.PyListToList(output);
            return results;
        }

        public int[] GetClassLineNumber(string className)
        {
            string script = pyLibFolder + @"\class_finder.py";
            string output = GetPyStdOutput(script, new string[] { className } );
            string[] nums = StringUtils.PyListToList(output);
            int[] result = new int[nums.Count()];

            for (int i = 0; i < result.Count(); i++)
            {
                Int32.TryParse(nums[i], out result[i]);
            }
            
            return result;
        }

        public void ParseTemplate(string className)
        {
            string script = pyLibFolder + @"\template_parser.py";
            string output = GetPyStdOutput(script, new string[] { className } );
        }

        private string GetPyStdOutput(string scriptPath)
        {
            Process p = new Process();
            p.StartInfo.FileName = "python.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = scriptPath + " " + this.fp;
            p.Start();
            StreamReader s = p.StandardOutput;
            string output = s.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        private string GetPyStdOutput(string scriptPath, string[] arguments)
        {
            Process p = new Process();
            p.StartInfo.FileName = "python.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = scriptPath + " " + this.fp;
            foreach (var item in arguments)
            {
                p.StartInfo.Arguments += " ";
                p.StartInfo.Arguments += item;
            }
            p.Start();
            StreamReader s = p.StandardOutput;
            string output = s.ReadToEnd();
            p.WaitForExit();

            string stdout = p.StandardError.ReadToEnd();
            return output;
        }
    }
}
