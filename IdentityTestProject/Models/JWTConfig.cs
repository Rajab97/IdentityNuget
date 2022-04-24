using PasswordDecryptorApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTestProject.Models
{
    public class JWTConfig
    {
        private string _secret;

        public string Secret
        {
            get
            {
                return _secret;
            }
            set
            {
                _secret = Decode.GetPassword(value);
            }
        }
        public string Module { get; set; }
    }
}
