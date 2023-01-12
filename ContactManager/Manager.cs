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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AddContact(string name, string firstName, string mail, string company, string link)
        {
            try
            {
                CurrentFolder.AddChild(name, firstName, mail, company, link);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // edition d'un contact
        public void EditContact(string name, string firstName)
        {
            string newName;
            string newFirstName;
            string newMail;
            string newCompany;
            string newLink;
            
            Console.WriteLine("Hit <RET> to keep the original.");
            Console.Write("new name: ");
            newName = Console.ReadLine();

            Console.Write("new firstname: ");
            newFirstName = Console.ReadLine();

            Console.Write("new mail: ");
            newMail = Console.ReadLine();

            Console.Write("new company: ");
            newCompany = Console.ReadLine();

            Console.Write("new link: ");
            newLink = Console.ReadLine();

            try
            {
                CurrentFolder.EditElement(name, firstName, newName, newFirstName, newMail, newCompany, newLink);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void EditFolder(string name)
        {
            string newName;

            Console.Write("new name: ");
            newName = Console.ReadLine();

            try
            {
                CurrentFolder.EditElement(name, newName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // suppression d'un sous fichier (recursif)
        public void RemoveElement(string name)
        {
            try
            {
                CurrentFolder.RemoveChild(name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // suppression d'un contact
        public void RemoveElement(string name, string firstname)
        {
            try
            {
                CurrentFolder.RemoveChild(name, firstname);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /*****************************************************************************/
        /* Chargement de fichier */

        public void LoadFile(string key)
        {
            LoadFile("contacts.xml", key); // fichier par defaut
        }

        public void LoadFile(string fileName, string key)
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
            SaveFile("contacts.xml", key); // fichier par defaut
        }

        public void SaveFile(string fileName, string key)
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
