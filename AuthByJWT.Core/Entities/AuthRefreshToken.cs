using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthByJWT.Core.Entities
{
    public class AuthRefreshToken
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration {  get; set; }
    }
}
