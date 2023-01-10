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
        public string     Name   { get { return _name; }    set { base.UpdateModificationDate(); _name = value; } }

        /*****************************************************************************/
        public Folder(string name, Folder parent) : base(parent)
        {
            SubFolders = new List<Folder>();
            Contacts = new List<Contact>();
            this.Name = name;
        }

        public Folder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _name = (string)info.GetValue("Name", typeof(string));
            Contacts = (List<Contact>)info.GetValue("contacts", typeof(List<Contact>));
            SubFolders = (List<Folder>)info.GetValue("subFolders", typeof(List<Folder>));
        }

        public Folder() : base(null) { }

        /*****************************************************************************/
        /* Ajout et suppression des dossier et des contactes:
         * Les contactes sont en debut de liste et le dossier en fin de liste pour rendre l'afficage avec `Show` plus clair
         */

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

        public void RemoveChild(string name)
        {
            // TODO
        }

        public void RemoveChild(string name, string firstName)
        {

        }

        /*****************************************************************************/
        /* Accessors */

        public Folder GetParent()
        {
            return base.Parent;
        }

        public Folder GetSubFolder(string name)
        {
            try
            {
                return SubFolders.FindAll(x => x.Name == name).First();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                throw new InvalidOperationException(name + " is not a valid folder name."); 
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                throw new InvalidOperationException(name + " is not a valid folder name."); 
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
