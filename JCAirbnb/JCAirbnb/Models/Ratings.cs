using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class Ratings
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Cleanliness")]
        public float Cleanliness { get; set; }

        [Display(Name = "Accuracy")]
        public float Accuracy { get; set; }

        [Display(Name = "Communication")]
        public float Communication { get; set; }

        [Display(Name = "Location")]
        public float Location { get; set; }

        [Display(Name = "Check-in")]
        public float CheckIn { get; set; }

        [Display(Name = "Value")]
        public float Value { get; set; }

        public float GetAverage()
        {
            return (Cleanliness + Accuracy + Communication + Location + CheckIn + Value) / 6.0f;
        }

        public override string ToString()
        {
            return GetAverage().ToString("F2");
        }
    }
}
