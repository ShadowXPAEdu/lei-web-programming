using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Models
{
    public class Reservation
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in date")]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-out date")]
        public DateTime CheckOut { get; set; }

        [Display(Name = "Reservation state")]
        public virtual ReservationState ReservationState { get; set; }

        [Display(Name = "Property")]
        public virtual Property Property { get; set; }

        [Display(Name = "Tenant")]
        public virtual IdentityUser User { get; set; }

        [Display(Name = "Reservation check list")]
        public virtual CheckList ReservationCheckList { get; set; }

        [Display(Name = "Delivery check list")]
        public virtual CheckList DeliveryCheckList { get; set; }

        [Display(Name = "Report")]
        public virtual Report Report { get; set; }
    }
}
