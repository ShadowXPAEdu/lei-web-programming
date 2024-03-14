using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Manager.Models
{
    public class ManagePropertyCreateViewModel
    {
        [Display(Name = "Property")]
        public JCAirbnb.Models.Property Property { get; set; }

        [Display(Name = "PropertyTypes")]
        public List<JCAirbnb.Models.PropertyType> PropertyTypes { get; set; }
    }
}
