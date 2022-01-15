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

        public static Ratings operator +(Ratings a, Ratings b)
        {
            if (b.Cleanliness != 0)
            {
                a.Cleanliness += b.Cleanliness;
                a.Cleanliness /= 2.0f;
            }
            if (b.Communication != 0)
            {
                a.Communication += b.Communication;
                a.Communication /= 2.0f;
            }
            if (b.CheckIn != 0)
            {
                a.CheckIn += b.CheckIn;
                a.CheckIn /= 2.0f;
            }
            if (b.Accuracy != 0)
            {
                a.Accuracy += b.Accuracy;
                a.Accuracy /= 2.0f;
            }
            if (b.Location != 0)
            {
                a.Location += b.Location;
                a.Location /= 2.0f;
            }
            if (b.Value != 0)
            {
                a.Value += b.Value;
                a.Value /= 2.0f;
            }

            return a;
        }

        public override string ToString()
        {
            return GetAverage().ToString("F2");
        }
    }
}
