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
    }
}
