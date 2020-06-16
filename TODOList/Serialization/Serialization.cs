using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    public static class Serialization
    {

        public static string path = ConfigurationManager.AppSettings["SerializationPath"];

        /// <summary>
        /// Serialize tasks
        /// </summary>
        public static void SaveToXml(ObservableCollection<TaskViewModel> main)
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<TaskViewModel>));

            FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, main);
            file.Close();
        }
        /// <summary>
        /// Deserialize tasks
        /// </summary>
        public static ObservableCollection<TaskViewModel> LoadFromXml()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<TaskViewModel>));
            System.IO.StreamReader file = new StreamReader(path);
            var ret = reader.Deserialize(file) as ObservableCollection<TaskViewModel>;
            file.Close();
            
            return ret;
        }
    }
}
