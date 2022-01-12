using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class CheckListItem
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Verified")]
        public bool Verified { get; set; } = false;

        [Display(Name = "Check list")]
        public virtual CheckList CheckList { get; set; }
    }
}
