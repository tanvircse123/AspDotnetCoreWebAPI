using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Model.Dto
{
    public class AuthenticationRequestBody
    {
        public string UserName {get;set;}
        public string Password {get;set;}
    }
}