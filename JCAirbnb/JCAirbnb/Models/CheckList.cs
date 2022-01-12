using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class CheckList
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
    }
}
