using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class Review
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Review")]
        public string Text { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Review date")]
        public DateTime Date { get; set; }

        [Display(Name = "Reviewer")]
        public virtual IdentityUser User { get; set; }
    }
}
