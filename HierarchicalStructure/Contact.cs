﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HierarchicalStructure
{
    public enum Links
    {
        Friend,
        Collegue,
        Relation,
        Network,
        None
    };

    [Serializable()]
    public class Contact : Node, ISerializable
    {
        private string _name;
        private string _firstName;
        private string _mail;
        private string _company;
        private Links _link;
        private int _id;
        private static int _counter = 0; // id builder

        public string Name { get { return _name; } set { base.UpdateModificationDate(); _name = value; } }
        public string FirstName { get { return _firstName; } set { base.UpdateModificationDate(); _firstName = value; } }
        public string Mail { get { return _mail; } set { _mail = ValidateMail(value); } }
        public string Company { get { return _company; } set { base.UpdateModificationDate(); _company = value; } }
        public Links Link { get { return _link; } set { base.UpdateModificationDate(); _link = value; } }
        public int Id { get { return _id; } }

        /*****************************************************************************/

        public Contact(string name, string firstName, string mail, string company, string link, Folder parent) : base(parent)
        {
            _name = name;
            _firstName = firstName;
            _company = company;
            _link = ToLinks(link);
            _id = _counter;
            _counter++;

            try
            {
                _mail = ValidateMail(mail);
            }
            catch (NonValidMailAddressException e)
            {
                Console.WriteLine("The given address is invalide, the field will be null.");
            }
            base.UpdateModificationDate();
        }

        // constructeur pour deserialisation
        public Contact(SerializationInfo info, StreamingContext context): base(info, context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            FirstName = (string)info.GetValue("firstName", typeof(string));
            Mail = (string)info.GetValue("mail", typeof(string));
            Company = (string)info.GetValue("company", typeof(string));
            Link = (Links)info.GetValue("link", typeof(Links));
            _id = (int)info.GetValue("id", typeof(int));
        }

        /*****************************************************************************/

        // création d'un lien à partir d'un string
        private Links ToLinks(string s)
        {
            Links link = Links.None;

            switch (s)
            {
                case "Friend":
                    link = Links.Friend;
                    break;
                case "Collegue":
                    link = Links.Collegue;
                    break;
                case "Relation":
                    link = Links.Relation;
                    break;
                case "Network":
                    link = Links.Network;
                    break;
                default:
                    Console.WriteLine("Error: invalid link, will be None bay default.");
                    break;
            }

            return link;
        }

        /*****************************************************************************/

        /* Valide la nouvelle addresse: si l'adresse n'est pas valide, on lève un exception et on ne fait pas la modification. */
        private string ValidateMail(string address)
        {
            Regex validEmail = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            string outputAdress;

            if (validEmail.IsMatch(address))
            {
                base.UpdateModificationDate();
                outputAdress = address;
            }
            else
            {
                outputAdress = _mail;
                throw new NonValidMailAddressException("Error: \"{0}\" is not a valid mail address." + address);
            }
            return outputAdress;
        }

        /*****************************************************************************/

        public override void PrettyPrint(int n)
        {
            base.PrettyPrintBar(n);
            Console.WriteLine(this.ToString());
        }

        /*****************************************************************************/

        public override string ToString()
        {
            return "contact: " + _name + ", " + _firstName + ", " + _mail + ", " + _company + ", " + Link.ToString() + ", " + base.ToString();
        }

        /*****************************************************************************/
        // sérialisation

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("firstName", FirstName);
            info.AddValue("mail", Mail);
            info.AddValue("company", Company);
            info.AddValue("link", Link);
            info.AddValue("id", Id);
            base.GetObjectData(info, context);
        }

    }
}
