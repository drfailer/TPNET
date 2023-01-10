using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace HierarchicalStructure
{
    [XmlInclude(typeof(Folder))]
    [XmlInclude(typeof(Contact))]
    [Serializable()]
    public abstract class Node: ISerializable
    {
        private DateTime _creationDate;
        private DateTime _modificationDate;

        protected DateTime CreationDate { get { return _creationDate; } }
        protected DateTime ModificationDate { get { return _modificationDate; } }
        [XmlIgnore]
        public Folder Parent { get; set; }
        public string NodeID { get; set; }
        // NOTE: je considère que le nom d'une personne est différent d'un Nom
        // de dossier (même nom, même type mais c'est pas vraiment la même chose
        // puisqu'un nom de personne ne sert à rien sans le prenom). Donc je ne
        // le met pas dans la classe mère.

        protected Node(Folder parent)
        {
            _creationDate = DateTime.Now;
            _modificationDate = DateTime.Now;
            Parent = parent;
        }

        // protected Node() { }

        protected Node(SerializationInfo info, StreamingContext context)
        {
            _creationDate = (DateTime) info.GetValue("creationDate", typeof(DateTime));
            _modificationDate = (DateTime) info.GetValue("modificationDate", typeof(DateTime));
            Parent = (Folder) info.GetValue("parent", typeof(Folder));
        }

        protected void UpdateModificationDate()
        {
            _modificationDate = DateTime.Now;
        }

        /*****************************************************************************/
        /* Fonctions d'affichage */

        public abstract void PrettyPrint(int n);

        // affiche les barres pour la commande tree
        protected void PrettyPrintBar(int n)
        {
            for (int i = 0; i < n; ++i)
            {
                Console.Write("\u2502 ");
            }
        }

        /*****************************************************************************/

        public override string ToString()
        {
            return "[" + _creationDate + ", " + _modificationDate + "]";
        }

        /*****************************************************************************/
        /* Fonction pour la sérialisation */

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("creationDate", _creationDate);
            info.AddValue("modificationDate", _modificationDate);
            info.AddValue("parent", Parent);
        }
    }
}
