using System;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class Commodity
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Commodity")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
