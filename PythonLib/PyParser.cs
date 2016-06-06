using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace PythonLib
{
    class PyParser
    {

        private string fp;
        
        public PyParser(string FilePath)
        {
            this.fp = FilePath;
        }

        public string[] GetTemplates()
        {
            Process p = new Process();
            p.StartInfo.FileName = "python.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            string script = @"""E:\Projects\microsoft\Norne Beta\Libs\Scripts\template_parser.py""";
            p.StartInfo.Arguments = script + " " + this.fp;
            p.Start();
            StreamReader s = p.StandardOutput;
            String output = s.ReadToEnd();
            p.WaitForExit();

            output = output.Replace("\r\n", string.Empty);
            output = output.Trim(new Char[] { '[', ']' });
            Console.WriteLine(output);

            string[] tList = output.Split(new Char[] { ',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            return tList;

        }
    }
}
