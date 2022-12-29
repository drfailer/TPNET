using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HierarchicalStructure;

namespace ContactManager
{
    internal class Manager
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
            CurrentFolder.AddChild(name);
        }

        public void AddContact(string name, string firstName, string mail, string company, Links link)
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
            throw new NotImplementedException();
        }

        /*****************************************************************************/
        /* Sauvegarde de fichiers */

        public void SaveFile()
        {
            SaveFile("contacts.xml");
        }

        public void SaveFile(string fileName)
        {
            throw new NotImplementedException();
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
