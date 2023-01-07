using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HierarchicalStructure;
using Serializer;

namespace ContactManager
{
    public class Manager
    {
        private Folder _root;
        private Folder CurrentFolder { get; set; }

        public Manager()
        {
            _root = new Folder("/", null);
            CurrentFolder = _root;
        }

        /*****************************************************************************/
        /* Fonctions d'ajout d'éléments */

        public void AddFolder(string name)
        {
            try
            {
                CurrentFolder.AddChild(name);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
        }

        public void AddContact(string name, string firstName, string mail, string company, string link)
        {
            CurrentFolder.AddChild(name, firstName, mail, company, link);
        }

        /*****************************************************************************/
        /* Move in the hierarchical structure */

        public void GoUp()
        {
            if (CurrentFolder != _root)
            {
                CurrentFolder = CurrentFolder.GetParent();
            }
            else
            {
                Console.WriteLine("Root n'a pas de parent!");
            }
        }

        public void GoDown(string name)
        {
            try
            {
                CurrentFolder = CurrentFolder.GetSubFolder(name);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // TODO
        public void EditContact(string contactName)
        {
            throw new NotImplementedException();
        }

        // TODO: remove a contact or a folder (recursive !)
        public void RemoveElement(string name)
        {
            throw new NotImplementedException();
        }

        /*****************************************************************************/
        /* Chargement de fichier */

        public void LoadFile(string key)
        {
            LoadFile("contacts.xml", key);
        }

        public void LoadFile(string fileName, string key) // todo: error handeling
        {
            try
            {
                string extention = fileName.Split('.').Last();
                CMSerializer serializer = new CMSerializer();
                _root = serializer.Deserialize(fileName, key);
                CurrentFolder = _root;
            }
            catch
            {
                Console.WriteLine("EREUR: impossible de charger le fichier.");
            }
        }

        /*****************************************************************************/
        /* Sauvegarde de fichiers */

        public void SaveFile(string key)
        {
            SaveFile("contacts.xml", key);
        }

        public void SaveFile(string fileName, string key) // todo: error handeling
        {
            try
            {
                CMSerializer serializer = new CMSerializer();
                serializer.Serialize(_root, fileName, key);
            }
            catch
            {
                Console.WriteLine("ERREUR: impossible de sauvegarder le fichier.");
            }
            
        }

        /*****************************************************************************/
        /* Fonctions d'affichage */

        public void ListCurrentFolder()
        {
            CurrentFolder.ListContent();
        }

        public void Display()
        {
            _root.PrettyPrint();
        }
    }
}
