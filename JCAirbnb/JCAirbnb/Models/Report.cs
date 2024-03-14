using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCAirbnb.Models
{
    public class Report
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Observations")]
        public string Observations { get; set; }

        [Display(Name = "Photos")]
        public virtual List<Photo> Photos { get; set; }

        //public string ReservationId { get; set; }

        //[ForeignKey("ReservationId")]
        //public virtual Reservation Reservation { get; set; }
    }
}
