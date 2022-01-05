﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class Company
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public virtual IdentityUser Manager { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}
