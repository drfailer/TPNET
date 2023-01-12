using System;
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

        public string Name { get { return _name; } set { base.UpdateModificationDate(); _name = value; } }
        public string FirstName { get { return _firstName; } set { base.UpdateModificationDate(); _firstName = value; } }
        public string Mail { get { return _mail; } set { _mail = ValidateMail(value); } }
        public string Company { get { return _company; } set { base.UpdateModificationDate(); _company = value; } }
        public Links Link { get { return _link; } set { base.UpdateModificationDate(); _link = value; } }

        /*****************************************************************************/

        public Contact(string name, string firstName, string mail, string company, string link, Folder parent) : base(parent)
        {
            if (name.Length == 0 || firstName.Length == 0) // vérfication nom valide
                throw new InvalidOperationException("Error: you must specify a correct name.");

            _name = name;
            _firstName = firstName;
            _company = company;
            _link = ToLinks(link);

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
        public Contact(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _name = (string)info.GetValue("Name", typeof(string));
            _firstName = (string)info.GetValue("firstName", typeof(string));
            _mail = (string)info.GetValue("mail", typeof(string));
            _company = (string)info.GetValue("company", typeof(string));
            _link = (Links)info.GetValue("link", typeof(Links));
        }

        public Contact() : base(null) { }

        /*****************************************************************************/

        // création d'un lien à partir d'un string
        public static Links ToLinks(string s)
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
            base.GetObjectData(info, context);
        }

    }
}