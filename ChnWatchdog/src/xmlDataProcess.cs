using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChnWatchdog.src
{
    class xmlDataProcess
    {
        // Config
        private XmlSerializer serialzer = null;
        private FileStream stream = null;

        // Create
        public void XMLCreate(xmlDataConfig config, string filepath)
        {
            XmlSerializer writer = new XmlSerializer(typeof(xmlDataConfig));
            using (StreamWriter sw = new StreamWriter(new FileStream(filepath, FileMode.Create), Encoding.UTF8))
            {
                writer.Serialize(sw, config);
            }

        }

        // Save
        public void XMLSerialize(xmlDataConfig config, string filepath)
        {
            serialzer = new XmlSerializer(typeof(xmlDataConfig));
            TextWriter writer = new StreamWriter(filepath);
            serialzer.Serialize(writer, config);
            writer.Close();
        }

        // Load
        public xmlDataConfig XMLDeserialize(string filePath, xmlDataConfig config)
        {
            serialzer = new XmlSerializer(typeof(xmlDataConfig));
            stream = new FileStream(filePath, FileMode.Open);
            config = (xmlDataConfig)serialzer.Deserialize(stream);
            stream.Close();

            return config;
        }
    }
}
