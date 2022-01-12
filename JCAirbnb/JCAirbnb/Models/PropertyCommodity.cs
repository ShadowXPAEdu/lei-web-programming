using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class PropertyCommodity
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Commodity")]
        public virtual Commodity Commodity { get; set; }

        [Display(Name = "Included")]
        public bool Included { get; set; } = true;

        public override bool Equals(object obj)
        {
            return obj is PropertyCommodity commodity &&
                   EqualityComparer<Commodity>.Default.Equals(Commodity, commodity.Commodity);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Commodity, Included);
        }
    }
}
