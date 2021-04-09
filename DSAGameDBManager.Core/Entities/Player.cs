using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSAGameDBManager.Core.Entities
{
    public class Player : IdentityUser
    {
        public string FullName { get; set; }

        public int GP { get; set; }
    }
}
