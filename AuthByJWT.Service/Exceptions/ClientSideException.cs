using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Service.Exceptions
{
    public class ClientSideException : Exception
    {
        public ClientSideException(string error) : base(error) { }
    }
}
