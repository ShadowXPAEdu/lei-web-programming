using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCAirbnb.Models
{
    public class Client
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [ForeignKey("Id")]
        [Display(Name = "Client")]
        public virtual IdentityUser User { get; set; }

        [Display(Name = "Reviews")]
        public virtual List<Review> Reviews { get; set; } = new();
    }
}
