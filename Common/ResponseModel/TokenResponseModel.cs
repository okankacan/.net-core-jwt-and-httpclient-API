using System;
using System.Collections.Generic;

namespace Common.ResponseModel
{
    public class TokenResponseModel
    {
        public string Token { get; set; }

        public DateTime ValidTo { get; set; }

        public IEnumerable<ClaimResponseModel> Claims { get; set; }
    }

    public class ClaimResponseModel
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }
}
