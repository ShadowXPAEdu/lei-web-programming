using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class Review
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string ClientId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
