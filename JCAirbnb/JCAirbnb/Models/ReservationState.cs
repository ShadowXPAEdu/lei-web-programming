using System;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class ReservationState
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "State")]
        public string Title { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ReservationState state &&
                   Id == state.Id &&
                   Title == state.Title;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title);
        }
    }
}
