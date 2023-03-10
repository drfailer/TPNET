using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace HierarchicalStructure
{
    [Serializable()]
    public class Folder : Node, ISerializable
    {
        public List<Folder> SubFolders { get; set; }
        public List<Contact> Contacts { get; set; }
        private string _name;
        public string Name { get { return _name; } set { base.UpdateModificationDate(); _name = value; } }

        /*****************************************************************************/
        /* Constucteurs */

        public Folder(string name, Folder parent) : base(parent)
        {
            if (name.Length == 0)
                throw new InvalidOperationException("Error: you must specify a valid name");
            SubFolders = new List<Folder>();
            Contacts = new List<Contact>();
            this.Name = name;
        }

        // constructeur pour deserialisation
        public Folder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _name = (string)info.GetValue("Name", typeof(string));
            Contacts = (List<Contact>)info.GetValue("contacts", typeof(List<Contact>));
            SubFolders = (List<Folder>)info.GetValue("subFolders", typeof(List<Folder>));
        }

        // constructeur pour deserialisation xml
        public Folder() : base(null) { }

        /*****************************************************************************/
        /* Ajout et suppression des dossier et des contactes:
         * Les contactes sont en debut de liste et le dossier en fin de liste pour rendre l'afficage avec `Show` plus clair
         */

        // Ajout d'un nouveau dossier
        public void AddChild(string name)
        { // ajout d'un dossier si le nom est libre
            if (SubFolders.Find(x => x.Name == name) == null)
            {
                SubFolders.Add(new Folder(name, this));
                base.UpdateModificationDate();
            }
            else
            {
                throw new InvalidOperationException(name + " already exists.");
            }
        }

        // Ajout d'un nouveau contact
        public void AddChild(string name, string firstName, string mail, string company, string link)
        {
            if (Contacts.Find(x => x.Name == name && x.FirstName == firstName) == null)
            { // ajout si nom libre
                Contacts.Add(new Contact(name, firstName, mail, company, link, this));
                base.UpdateModificationDate();
            }
            else
            {
                throw new InvalidOperationException(name + " " + firstName + " already exists.");
            }
        }

        // Suppression d'un dossier (throws ArgumentNullException)
        public void RemoveChild(string name)
        {
            SubFolders.RemoveAll(x => x.Name == name);
        }

        // supperssion d'un contact (throws ArgumentNullException)
        public void RemoveChild(string name, string firstName)
        {
            Contacts.RemoveAll(x => x.Name == name && x.FirstName == firstName);
        }

        /*****************************************************************************/
        /* Fonctions d'édition */

        // permet la modification d'un contact (les nouveau champs doivent être non vides)
        public void EditElement(string originalName, string originalFirstName,
            string name, string firstName, string mail, string company, string link)
        {
            Contact contact = Contacts.Find(x => x.Name == originalName && x.FirstName == originalFirstName);
            if (contact == null)
            {
                throw new InvalidOperationException("impossible to change: " + originalName + " " + originalFirstName);
            }
            else
            {
                if (name.Length != 0)
                    contact.Name = name;
                if (firstName.Length != 0)
                    contact.FirstName = firstName;
                if (mail.Length != 0)
                    contact.Mail = mail;
                if (company.Length != 0)
                    contact.Company = company;
                if (link.Length != 0)
                    contact.Link = Contact.ToLinks(link);
            }
        }

        // permet de changer le nom d'un dossier
        public void EditElement(string originalName, string newName)
        {
            Folder folder = SubFolders.Find(x => x.Name == originalName);
            if (folder == null)
            {
                throw new InvalidOperationException("Error: impossible to change the name of: " + originalName);
            }
            else
            {
                folder.Name = newName;
            }
        }

        /*****************************************************************************/
        /* Accessors */

        // retourne le dossier parent
        public Folder GetParent()
        {
            return base.Parent;
        }

        // retourne le dossier portant le nom `name`
        public Folder GetSubFolder(string name)
        {
            try
            {
                return SubFolders.FindAll(x => x.Name == name).First();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error: " + name + " does not exist.");
            }
        }

        /*****************************************************************************/
        /* Fonctions d'affichage */

        /* Fait un pretty print du dossier et des sous dossiers */
        public override void PrettyPrint(int n = 0)
        {
            base.PrettyPrintBar(n);
            Console.WriteLine("\u250C " + this.ToString());
            SubFolders.ForEach(x => { x.PrettyPrint(n + 1); });
            Contacts.ForEach(x => { x.PrettyPrint(n + 1); });
            base.PrettyPrintBar(n);
            Console.WriteLine("\u2514\u2500\u2500");
        }

        /* Liste le contenu d'un dossier */
        public void ListContent()
        {
            Console.WriteLine(this.ToString());
            SubFolders.ForEach(x =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(x.Name);
                Console.ForegroundColor = ConsoleColor.Gray;
            });
            Contacts.ForEach(x => Console.WriteLine(x.ToString()));
        }

        /*****************************************************************************/

        public override string ToString()
        {
            return Name + " - " + base.ToString();
        }

        /*****************************************************************************/
        // sérialisation

        // #ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("contacts", Contacts);
            info.AddValue("subFolders", SubFolders);
            base.GetObjectData(info, context);
        }

        // mise à jour des pere pour la deserialisation xml
        public void UpdateParent()
        {
            Contacts.ForEach(x => x.Parent = this);
            SubFolders.ForEach(x =>
            {
                x.Parent = this; // Node
                x.UpdateParent(); // Folder
            });
        }
    }
}
