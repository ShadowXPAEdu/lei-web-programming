using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string ReservationStateId { get; set; }

        [ForeignKey("ReservationStateId")]
        [Display(Name = "Reservation state")]
        public virtual ReservationState ReservationState { get; set; }

        public string PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        [Display(Name = "Property")]
        public virtual Property Property { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [Display(Name = "Tenant")]
        public virtual IdentityUser User { get; set; }

        public string ReservationCheckListId { get; set; }

        [ForeignKey("ReservationCheckListId")]
        [Display(Name = "Reservation check list")]
        public virtual CheckList ReservationCheckList { get; set; }

        public string DeliveryCheckListId { get; set; }

        [ForeignKey("DeliveryCheckListId")]
        [Display(Name = "Delivery check list")]
        public virtual CheckList DeliveryCheckList { get; set; }

        public string ReportId { get; set; }

        [ForeignKey("ReportId")]
        [Display(Name = "Report")]
        public virtual Report Report { get; set; }
    }
}
