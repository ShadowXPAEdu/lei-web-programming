using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCAirbnb.Models
{
    public class Divisions
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Guest(s)")]
        public int Guest { get; set; } = 0;

        [Display(Name = "Bedroom(s)")]
        public int Bedroom { get; set; } = 0;

        [Display(Name = "Bed(s)")]
        public int Bed { get; set; } = 0;

        [Display(Name = "Bath(s)")]
        public int Bath { get; set; } = 0;

        [Display(Name = "Private Bath(s)")]
        public int PrivateBath { get; set; } = 0;
    }
}
