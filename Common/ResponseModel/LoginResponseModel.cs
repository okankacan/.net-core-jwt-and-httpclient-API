using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JWT.Enums.Common;

namespace Common.ResponseModel
{
    public class LoginResponseModel : TokenResponseModel
    {
        public LoginStatus Status { get; set; }
    }
}
