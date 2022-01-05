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

        public List<string> Photos { get; set; }

        public virtual List<Review> Reviews { get; set; }
        public Rating Rating { get; set; }
    }
}
