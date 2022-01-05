using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class Commodity
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
