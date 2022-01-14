using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class Report
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Observations")]
        public string Observations { get; set; }

        [Display(Name = "Photos")]
        public virtual List<Photo> Photos { get; set; }
    }
}
