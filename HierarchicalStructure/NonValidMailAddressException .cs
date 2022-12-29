using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalStructure
{
    public class NonValidMailAddressException : Exception
    {
        public NonValidMailAddressException(string msg): base(msg)
        {
        }
    }
}
