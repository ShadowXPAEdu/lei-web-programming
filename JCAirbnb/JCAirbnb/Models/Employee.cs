using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class Employee
    {
        [Key]
        public string Id { get; set; }

        public IdentityUser User { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Employee employee &&
                   EqualityComparer<IdentityUser>.Default.Equals(User, employee.User);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, User);
        }
    }
}
