using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class Reservation
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        public string ReservationStateId { get; set; }
        public virtual ReservationState ReservationState { get; set; }

        public string PropertyId { get; set; }
        public virtual Property Property { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
