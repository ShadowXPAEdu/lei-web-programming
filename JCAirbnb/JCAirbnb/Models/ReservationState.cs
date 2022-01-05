using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class ReservationState
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
