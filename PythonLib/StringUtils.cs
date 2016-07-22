using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Norne_Beta.UIElements;
using Newtonsoft.Json.Linq;


namespace PythonLib
{
    class StringUtils
    {

        static public ObservableCollection<ListItem> PyListToObservableCollection(string pyString)
        {
            string[] list = PyListToList(pyString);

            ObservableCollection<ListItem> ret = new ObservableCollection<ListItem>();
            foreach (string item in list)
            {
                ret.Add(new ListItem(item));
            }

            return ret;
        }

        static public ObservableCollection<ListItem> JArrayStringToObservableCollection(JArray array)
        {
            ObservableCollection<ListItem> ret = new ObservableCollection<ListItem>();
            foreach (string item in array)
            {
                ret.Add(new ListItem(item));
            }
            return ret;
        }

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

        static public ObservableCollection<ListItem> ListToObservableCollection(string[] list)
        {
            ObservableCollection<ListItem> ret = new ObservableCollection<ListItem>();
            foreach (string item in list)
            {
                ret.Add(new ListItem(item));
            }

            return ret;
        }

        static public string ObservableCollectionToPyList<T>(ObservableCollection<T> list) where T : ListItem
        {
            string ret = String.Empty;
            ret += "[";

            foreach (var item in list)
            {
                ret += "\"" + item.Label.ToString() + "\", ";
            }
            ret += "]";

            return ret;
        }

        static public string ListStringToPyList(List<string> listString)
        {
            string pyString = "[";

            foreach (string item in listString)
            {
                pyString += "\"" + item + "\"" + ",";
            }
            pyString += "]";

            return pyString;
        }

    }
}
