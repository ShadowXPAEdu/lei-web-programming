using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCAirbnb.Models
{
    public class Employee
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        //public string UserId { get; set; }

        [Required]
        //[ForeignKey("UserId")]
        [Display(Name = "User")]
        public virtual IdentityUser User { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Employee employee &&
                   Id == employee.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, User);
        }
    }
}
