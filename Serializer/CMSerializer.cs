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
    public class CMSerializer
    {
        public CMSerializer() { }

        /*****************************************************************************/
        /* fonctions de seraialisation et de deserialisation publiques qui utilise les
         * bonnes methodes en fonction du type de fichier demandé
         */

        public void Serialize(Folder root, string fileName)
        {
            string extention = fileName.Split('.').Last();

            try
            {
                if (extention == "dat")
                {
                    SerializeBin(root, fileName);
                }
                else
                {
                    SerializeXML(root, fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception();
            }
        }

        public Folder Deserialize(string fileName)
        {
            string extention = fileName.Split('.').Last();
            Folder root = null;

            try
            {
                if (extention == "dat")
                {
                    root = DeserializeBin(fileName);
                }
                else
                {
                    root = DeserializeXML(fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception();
            }
            return root;
        }

        /*****************************************************************************/
        // Sérialisation binaire

        private void SerializeBin(Folder root, string fileName)
        {
            Stream stream = File.Open(fileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, root);
            stream.Close();
        }

        private Folder DeserializeBin(string fileName)
        {
            Folder root = null;
            Stream stream = File.Open(fileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            root = (Folder)bf.Deserialize(stream);
            stream.Close();
            return root;
        }

        /*****************************************************************************/
        // sérialisation XML

        private void SerializeXML(Folder root, string fileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Node));
            TextWriter tw = new StreamWriter(@"C:\Users\rechassagn1\Documents" + fileName);
            xmlSerializer.Serialize(tw, root);
            tw.Close();
        }

        private Folder DeserializeXML(string fileName)
        {
            Folder root;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Node));
            TextReader tr = new StreamReader(@"C:\Users\rechassagn1\Documents" + fileName);
            root = (Folder)xmlSerializer.Deserialize(tr);
            tr.Close();
            root.UpdateParent();
            return root;
        }
    }
}
