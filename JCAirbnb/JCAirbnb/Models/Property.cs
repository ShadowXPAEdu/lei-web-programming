using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public List<string> Photos { get; set; }

        public virtual List<Commodity> Commodities { get; set; }

        public virtual List<Commodity> NonIncludedCommodities { get; set; }

        public virtual List<Review> Reviews { get; set; }
    }
}
