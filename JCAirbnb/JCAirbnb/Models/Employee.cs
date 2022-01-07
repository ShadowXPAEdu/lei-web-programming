using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class Employee
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "User")]
        public virtual IdentityUser User { get; set; }

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
