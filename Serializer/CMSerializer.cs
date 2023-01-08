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
        private delegate void SerializeMethod(CryptoStream cs, Folder folder);
        private delegate Folder DeserializeMethod(CryptoStream cs);

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

                if (extention == "dat")
                {
                    // SerializeBin(root, fileName, bkey);
                    SecureSerialize(root, fileName, bkey, BinarySerializer);
                }
                else
                {
                    // SerializeXML(root, fileName, bkey);
                    SecureSerialize(root, fileName, bkey, XMLSerializer);
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

                if (extention == "dat")
                {
                    // root = DeserializeBin(fileName, bkey);
                    root = SecureDeserialize(fileName, bkey, BinaryDeserializer);
                }
                else
                {
                    // root = DeserializeXML(fileName, bkey);
                    root = SecureDeserialize(fileName, bkey, XMLDeserializer);
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
        // Sérialisation et deserialisation sécurisée

        private void SecureSerialize(Folder root, string fileName, byte[] key, SerializeMethod serializer)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                using (FileStream fs = File.Open(@"C:\Users\Megaport\Documents\" + fileName, FileMode.Create))
                {
                    fs.Write(aes.IV, 0, aes.IV.Length); // sauvegarde du vecteur d'initialisation en debut de fichier
                    using (CryptoStream csEncrypt = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        serializer(csEncrypt, root);
                    }
                }
            }
        }

        private Folder SecureDeserialize(string fileName, byte[] key, DeserializeMethod deserializer)
        {
            Folder root = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;

                using (FileStream fs = File.Open(@"C:\Users\Megaport\Documents\" + fileName, FileMode.Open))
                {
                    // récupération du vecteur d'initialisation utilisé au chiffrement
                    byte[] buff = new byte[aes.IV.Length];
                    fs.Read(buff, 0, aes.IV.Length);
                    aes.IV = buff;
                    using (CryptoStream csDecrypt = new CryptoStream(fs, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
                    {
                        root = deserializer(csDecrypt);
                    }
                }
            }
            return root;
        }

        /*****************************************************************************/
        /* Binary seralizer and deserializer */

        private void BinarySerializer(CryptoStream cs, Folder folder)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(cs, folder);
        }

        private Folder BinaryDeserializer(CryptoStream cs)
        {
            Folder root;
            BinaryFormatter bf = new BinaryFormatter();
            root = (Folder)bf.Deserialize(cs);
            // root.UpdateParent();
            return root;
        }

        /*****************************************************************************/
        /* XML seralizer and deserializer */

        private void XMLSerializer(CryptoStream cs, Folder folder)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Node));
            xmlSerializer.Serialize(cs, folder);
        }

        private Folder XMLDeserializer(CryptoStream cs)
        {
            Folder root;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Node));
            root = (Folder)xmlSerializer.Deserialize(cs);
            root.UpdateParent(); // ici, on doit mettre à jour les parent à la main
            return root;
        }
    }
}
