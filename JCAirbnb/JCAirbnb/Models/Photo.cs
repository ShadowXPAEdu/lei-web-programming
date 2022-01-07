using System;
using System.ComponentModel.DataAnnotations;

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
    }
}
