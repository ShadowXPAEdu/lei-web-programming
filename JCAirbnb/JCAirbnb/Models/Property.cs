using Microsoft.AspNetCore.Identity;
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

        public string RatingsId { get; set; }

        [ForeignKey("RatingsId")]
        [Display(Name = "Ratings")]
        public virtual Ratings Ratings { get; set; }

        public string DivisionsId { get; set; }

        [ForeignKey("DivisionsId")]
        [Display(Name = "Divisions")]
        public virtual Divisions Divisions { get; set; }

        [Display(Name = "Photos")]
        public virtual List<Photo> Photos { get; set; } = new();

        public string ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        [Display(Name = "Host")]
        public virtual IdentityUser Manager { get; set; }

        [Display(Name = "Commodities")]
        public virtual List<PropertyCommodity> Commodities { get; set; } = new();

        [Display(Name = "Reviews")]
        public virtual List<Review> Reviews { get; set; } = new();

        public string PropertyTypeId { get; set; }

        [ForeignKey("PropertyTypeId")]
        [Display(Name = "Property type")]
        public virtual PropertyType PropertyType { get; set; }
    }
}
