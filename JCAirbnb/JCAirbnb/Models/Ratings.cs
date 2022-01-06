using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class Ratings
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Cleanliness")]
        public float Cleanliness { get; set; } = 0;

        [Display(Name = "Accuracy")]
        public float Accuracy { get; set; } = 0;

        [Display(Name = "Communication")]
        public float Communication { get; set; } = 0;

        [Display(Name = "Location")]
        public float Location { get; set; } = 0;

        [Display(Name = "Check-in")]
        public float CheckIn { get; set; } = 0;

        [Display(Name = "Value")]
        public float Value { get; set; } = 0;
    }
}
