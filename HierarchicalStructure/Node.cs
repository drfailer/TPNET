using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HierarchicalStructure
{
    [Serializable()]
    public abstract class Node: ISerializable
    {
        protected DateTime CreationDate { get; set; }
        protected DateTime ModificationDate { get; set; }
        protected Folder Parent { get; set; }
        // NOTE: je considère que le nom d'une personne est différent d'un Nom
        // de dossier (même nom, même type mais c'est pas vraiment la même chose
        // puisqu'un nom de personne ne sert à rien sans le prenom). Donc je ne
        // le met pas dans la classe mère.

        protected Node(Folder parent)
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            Parent = parent;
        }

        protected Node(SerializationInfo info, StreamingContext context)
        {
            CreationDate = (DateTime) info.GetValue("creationDate", typeof(DateTime));
            ModificationDate = (DateTime) info.GetValue("modificationDate", typeof(DateTime));
            Parent = (Folder) info.GetValue("parent", typeof(Folder));
        }

        protected void UpdateModificationDate()
        {
            ModificationDate = DateTime.Now;
        }

        /*****************************************************************************/
        /* Fonctions d'affichage */

        public abstract void PrettyPrint(int n);

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
            return "[" + CreationDate + ", " + ModificationDate + "]";
        }

        /*****************************************************************************/

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("creationDate", CreationDate);
            info.AddValue("modificationDate", ModificationDate);
            info.AddValue("parent", Parent);
        }
    }
}
