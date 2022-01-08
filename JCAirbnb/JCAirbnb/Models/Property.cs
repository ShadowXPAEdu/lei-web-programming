using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCAirbnb.Models
{
    public class Property
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "BasePrice")]
        public int BasePrice { get; set; }

        [Display(Name = "Price")]
        public int Price { get; set; }

        //[Required]
        [Display(Name = "Ratings")]
        public virtual Ratings Ratings { get; set; }

        //[Required]
        [Display(Name = "Divisions")]
        public virtual Divisions Divisions { get; set; }

        [Display(Name = "Photos")]
        public virtual List<Photo> Photos { get; set; } = new();

        //[Required]
        [Display(Name = "Host")]
        public virtual IdentityUser Manager { get; set; }

        [Display(Name = "Commodities")]
        public virtual List<PropertyCommodity> Commodities { get; set; } = new();

        [Display(Name = "Reviews")]
        public virtual List<Review> Reviews { get; set; } = new();

        [Display(Name = "Property type")]
        public virtual PropertyType PropertyType { get; set; }
    }
}
