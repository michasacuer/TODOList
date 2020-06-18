using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

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
            // widac co to za typ po prawej, po co te kilometry
            var writer = new XmlSerializer(typeof(ObservableCollection<TaskViewModel>));

            FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, main);
            file.Close();
        }
        /// <summary>
        /// Deserialize tasks
        /// </summary>
        public static ObservableCollection<TaskViewModel> LoadFromXml()
        {
            var reader = new XmlSerializer(typeof(ObservableCollection<TaskViewModel>));
            var file = new StreamReader(path);
            // as wywala nulla jak nie może zrzutować. Pisząc tak pokazujesz, że wiesz co robisz
            var ret = reader.Deserialize(file) as ObservableCollection<TaskViewModel> ?? throw new ArgumentNullException("msg");
            file.Close();

            return ret;
        }
    }
}
