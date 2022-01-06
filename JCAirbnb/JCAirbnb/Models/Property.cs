using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCAirbnb.Models
{
    public class Property
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int BasePrice { get; set; }

        public int Price { get; set; }

        public Ratings Ratings { get; set; }

        public Divisions Divisions { get; set; }

        public List<Photo> Photos { get; set; }

        public IdentityUser Manager { get; set; }

        public virtual List<PropertyCommodity> Commodities { get; set; }

        public virtual List<Review> Reviews { get; set; }
    }
}
