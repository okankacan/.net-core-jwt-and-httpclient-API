using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.RequestModel
{
    public class LoginRequestModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
