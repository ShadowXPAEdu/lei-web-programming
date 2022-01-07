using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Areas.Admin.Models
{
    public class ManageClientEditViewModel
    {
        [Display(Name = "Roles")]
        public List<IdentityRole> Roles { get; set; }

        [Display(Name = "Client")]
        public JCAirbnb.Models.Client Client { get; set; }

        [Display(Name = "AddingRole")]
        public bool AddingRole { get; set; }
    }
}
