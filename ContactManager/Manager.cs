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

        public void LoadFile(string fileName)
        {
            string extention = fileName.Split('.').Last();
            
            if (extention == "dat")
            {
                BinSerializer serializer = new BinSerializer();
                _root = serializer.Desirialisation(fileName);
            }
            else
            {
                Console.WriteLine("xml not working yet!");
            }
        }

        /*****************************************************************************/
        /* Sauvegarde de fichiers */

        public void SaveFile()
        {
            SaveFile("contacts.xml");
        }

        public void SaveFile(string fileName)
        {
            string extention = fileName.Split('.').Last();
            
            if (extention == "dat")
            {
                BinSerializer serializer = new BinSerializer();
                serializer.Serialisation(_root, fileName);
            }
            else
            {
                Console.WriteLine("xml not working yet!");
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
