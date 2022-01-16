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
            a.Cleanliness += b.Cleanliness;
            a.Communication += b.Communication;
            a.CheckIn += b.CheckIn;
            a.Accuracy += b.Accuracy;
            a.Location += b.Location;
            a.Value += b.Value;

            return a;
        }

        public static Ratings operator /(Ratings a, int b)
        {
            b++;
            if (b == 0) return a;
            a.Cleanliness /= b;
            a.Communication /= b;
            a.CheckIn /= b;
            a.Accuracy /= b;
            a.Location /= b;
            a.Value /= b;

            return a;
        }

        public override string ToString()
        {
            return GetAverage().ToString("F2");
        }
    }
}
