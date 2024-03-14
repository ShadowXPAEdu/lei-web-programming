using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCAirbnb.Models
{
    public class Photo
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Path")]
        public string Path { get; set; }

        //public string PropertyId { get; set; }

        //[ForeignKey("PropertyId")]
        //public virtual Property Property { get; set; }

        //public string ReportId { get; set; }

        //[ForeignKey("ReportId")]
        //public virtual Report Report { get; set; }
    }
}
