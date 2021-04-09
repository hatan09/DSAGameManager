using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSAGameDBManager.Api.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }
    }
}
