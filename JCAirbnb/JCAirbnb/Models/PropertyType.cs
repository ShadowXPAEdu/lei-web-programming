using System;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class PropertyType
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Type")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
