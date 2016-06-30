using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Norne_Beta.FileUtils
{
    class FileWriter
    {
        public void SerializeObject<T>(T serializableObject, FileStream fs)
        {
            if(serializableObject == null) { return; }

            XmlDocument xmlDocument = new XmlDocument();
            Type x = serializableObject.GetType();
            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            Console.WriteLine("HAHA");
            serializer.Serialize(fs, serializableObject);
        }
    }
}
