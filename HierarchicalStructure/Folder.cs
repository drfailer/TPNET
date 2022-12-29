﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalStructure
{
    public class Folder : Node
    {
        private List<Node> _childs;
        private string _name;

        public string Name
        {
            get { return _name; }
            set { base.UpdateModificationDate(); _name = value; }
        }

        /*****************************************************************************/
        public Folder(string name, Folder parent) : base(parent)
        {
            _childs = new List<Node>();
            this.Name = name;
        }

        /*****************************************************************************/
        /* Ajout des dossier et des contactes:
         * Les contactes sont en debut de liste et le dossier en fin de liste pour rendre l'afficage avec `Show` plus clair
         */

        public void AddChild(string name)
        {
            _childs.Add(new Folder(name, this));
        }

        public void AddChild(string name, string firstName, string mail, string company, Links link)
        {
            _childs.Insert(0, new Contact(name, firstName, mail, company, link, this));
        }

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
            return base.ToString() + " - " + Name;
        }
    }
}
