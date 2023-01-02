using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using HierarchicalStructure;

namespace Serializer
{
    public class BinSerializer
    {
        public BinSerializer() { }

        public void Serialisation(Folder root, string fileName)
        {
            Stream stream = File.Open(fileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, root);
            stream.Close();
        }

        public Folder Desirialisation(string fileName)
        {
            Folder root;
            Stream stream = File.Open(fileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            root = (Folder)bf.Deserialize(stream);
            stream.Close();
            return root;
        }
    }
}
