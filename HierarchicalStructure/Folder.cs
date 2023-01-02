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

namespace HierarchicalStructure
{
    [Serializable()]
    public class Folder : Node, ISerializable
    {
        private List<Node> _childs;
        private string _name;
        private string _id;

        public string Name
        {
            get { return _name; }
            set {
                base.UpdateModificationDate();
                if (Parent != null)
                {
                    _id = Parent.Id + value + "/";
                }
                else
                {
                    _id = value;
                }
                _name = value;
            }
        }

        public string Id { get { return _id; } }

        /*****************************************************************************/
        public Folder(string name, Folder parent) : base(parent)
        {
            _childs = new List<Node>();
            this.Name = name;
            if (parent != null)
            {
                _id = parent.Id + name + "/";
            }
            else
            {
                _id = name;
            }
        }

        public Folder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            _id = (string)info.GetValue("id", typeof(string));
            _childs = (List<Node>)info.GetValue("childs", typeof(List<Node>));
        }

        /*****************************************************************************/
        /* Ajout des dossier et des contactes:
         * Les contactes sont en debut de liste et le dossier en fin de liste pour rendre l'afficage avec `Show` plus clair
         */

        public void AddChild(string name)
        {
            if (_childs.Find(x => x.GetType() == typeof(Folder) && ((Folder)x).Name == name) == null)
            {
                _childs.Add(new Folder(name, this));
            }
            else
            {
                throw new InvalidOperationException(name + " already exists.");
            }
        }

        public void AddChild(string name, string firstName, string mail, string company, string link)
        {
            _childs.Insert(0, new Contact(name, firstName, mail, company, link, this));
        }

        // Pour la sérialisation

        public void AddChild(Folder newFolder)
        {
            _childs.Add(newFolder);
        }


/*        public void AddContact(string value)
        {
            _childs.Insert(0, new Contact(value, this));
        }
*/
        /* Accessors */

        public Folder GetParent()
        {
            return base.Parent;
        }

        public Folder GetSubFolder(string name)
        {
            try
            {
                return (Folder)_childs.FindAll(x => x.GetType() == typeof(Folder) && ((Folder)x).Name == name).First();
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

        public List<Node> GetChilds() // requis pour serialisation
        {
            return _childs;
        }

        /*****************************************************************************/
        /* Fonctions d'affichage */

        /* Fait un pretty print du dossier et des sous dossiers */
        public override void PrettyPrint(int n = 0)
        {
            base.PrettyPrintBar(n);
            Console.WriteLine("\u250C " + this.ToString());
            _childs.ForEach(x => { x.PrettyPrint(n + 1); });
            base.PrettyPrintBar(n);
            Console.WriteLine("\u2514\u2500\u2500");
        }

        /* Liste le contenu d'un dossier */
        public void ListContent()
        {
            Console.WriteLine("\u250C " + this.ToString());
            _childs.ForEach(x => Console.WriteLine("\u2502 " + x.ToString()));
            Console.WriteLine("\u2514\u2500\u2500");
        }

        /*****************************************************************************/

        public override string ToString()
        {
            return Name + " - " + base.ToString();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("id", Id);
            info.AddValue("childs", _childs);
            base.GetObjectData(info, context);
        }
    }
}
