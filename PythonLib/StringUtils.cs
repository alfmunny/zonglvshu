using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonLib
{
    class StringUtils
    {
        static public string[] PyListToList(string pyString)
        {
            pyString = pyString.Replace("\r\n", string.Empty);
            pyString = pyString.Trim(new Char[] { '[', ']' });

            string[] list = pyString.Split(new Char[] { ',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < list.Count(); i++)
            {
                list[i] = list[i].Trim(new Char[] { '\''} );
            }
           return list;
        }

    }
}
