using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalStructure
{
    public abstract class Node
    {
        private DateTime _creationDate;
        private DateTime _modificationDate;
        protected Folder Parent { get; set; }
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

        protected void UpdateModificationDate()
        {
            _modificationDate = DateTime.Now;
        }

        /*****************************************************************************/
        /* Getters */

        protected DateTime GetCreationDate()
        {
            return _creationDate;
        }

        protected DateTime GetModificationDate()
        {
            return _modificationDate;
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
            return "[" + _creationDate + ", " + _modificationDate + "]";
        }
    }
}
