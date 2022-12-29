using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

    public class Contact : Node
    {
        private string _name;
        private string _firstName;
        private string _mail;
        private string _company;
        private Links _link;

        public string Name
        {
            get { return _name; }
            set { base.UpdateModificationDate(); _name = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { base.UpdateModificationDate(); _firstName = value; }
        }
        public string Mail
        {
            get { return _mail; }
            set { _mail = ValidateMail(value); }
        }
        public string Company
        {
            get { return _company; }
            set { base.UpdateModificationDate(); _company = value; }
        }
        public Links Link
        {
            get { return _link; }
            set { base.UpdateModificationDate(); _link = value; }
        }

        /*****************************************************************************/

        public Contact(string name, string firstName, string mail, string company, Links link, Folder parent) : base(parent)
        {
            _name = name;
            _firstName = firstName;
            _company = company;
            _link = link;

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
            return "Contact: (" + _name + ", " + _firstName + ", " + _mail + ", " + _company + ", " + Link.ToString() + ", " + base.ToString() + ")";
        }
    }
}
