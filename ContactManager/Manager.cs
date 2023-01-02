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

        public void EditContact(string contactName)
        {
            throw new NotImplementedException();
        }

        /*****************************************************************************/
        /* Chargement de fichier */

        public void LoadFile()
        {
            LoadFile("contacts.xml");
        }

        public void LoadFile(string fileName) // todo: error handeling
        {
            string extention = fileName.Split('.').Last();
            CMSerializer serializer = new CMSerializer();
            _root = serializer.Deserialize(fileName);
        }

        /*****************************************************************************/
        /* Sauvegarde de fichiers */

        public void SaveFile()
        {
            SaveFile("contacts.xml");
        }

        public void SaveFile(string fileName) // todo: error handeling
        {
            CMSerializer serializer = new CMSerializer();
            serializer.Serialize(_root, fileName);
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
