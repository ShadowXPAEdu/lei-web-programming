using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class Company
    {
        public string ManagerId { get; set; }
        public virtual IdentityUser Manager { get; set; }

        public virtual List<IdentityUser> Employees { get; set; }
    }
}
