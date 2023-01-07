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
using System.Security.Cryptography;

namespace Serializer
{
    public class CMSerializer
    {
        public CMSerializer() { }

        /*****************************************************************************/
        /* fonctions de seraialisation et de deserialisation publiques qui utilise les
         * bonnes methodes en fonction du type de fichier demandé
         */

        public void Serialize(Folder root, string fileName, string key)
        {
            string extention = fileName.Split('.').Last();

            try
            {
                // On utilise le hash de la clé sous forme de string pour avoir une clé de la bonne taille
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(key));
                Array.Resize(ref hash, 16);
                byte[] bkey = hash;
                byte[] biv = hash; // WARNING: j'ai pas bien compris comment bien gérer le vecteur d'initialisation
                if (extention == "dat")
                {
                    SerializeBin(root, fileName, bkey, biv);
                }
                else
                {
                    SerializeXML(root, fileName, bkey, biv);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception();
            }
        }

        public Folder Deserialize(string fileName, string key)
        {
            string extention = fileName.Split('.').Last();
            Folder root = null;

            try
            {
                // On utilise le hash de la clé sous forme de string pour avoir une clé de la bonne taille
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(key));
                Array.Resize(ref hash, 16);
                byte[] bkey = hash;
                byte[] biv = hash; // WARNING: j'ai pas bien compris comment bien gérer le vecteur d'initialisation
                if (extention == "dat")
                {
                    root = DeserializeBin(fileName, bkey, biv);
                }
                else
                {
                    root = DeserializeXML(fileName, bkey, biv);
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

        private void SerializeBin(Folder root, string fileName, byte[] key, byte[] iv)
        {

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fs = File.Open(@"C:\Users\Megaport\Documents\" + fileName, FileMode.Create))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(csEncrypt, root);
                    }
                }
            }
        }

        private Folder DeserializeBin(string fileName, byte[] key, byte[] iv)
        {
            Folder root = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fs = File.Open(@"C:\Users\Megaport\Documents\" + fileName, FileMode.Open))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(fs, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        root = (Folder)bf.Deserialize(csDecrypt);
                        root.UpdateParent();
                    }
                }
            }
            return root;
        }

        /*****************************************************************************/
        // sérialisation XML

        private void SerializeXML(Folder root, string fileName, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fs = File.Open(@"C:\Users\Megaport\Documents\" + fileName, FileMode.Create))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Node));
                        xmlSerializer.Serialize(csEncrypt, root);
                    }
                }
            }
        }

        private Folder DeserializeXML(string fileName, byte[] key, byte[] iv)
        {
            Folder root;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fs = File.Open(@"C:\Users\Megaport\Documents\" + fileName, FileMode.Open))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(fs, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Node));
                        root = (Folder)xmlSerializer.Deserialize(csDecrypt);
                        root.UpdateParent();
                    }
                }
            }
            return root;
        }
    }
}
