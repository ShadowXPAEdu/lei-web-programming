using JCAirbnb.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Areas.Admin.Models
{
    public class ManageOwnerDetailsViewModel
    {
        [Display(Name = "Manager")]
        public IdentityUser Manager { get; set; }

        [Display(Name = "Properties")]
        public List<Property> Properties { get; set; }
    }
}
