﻿using System;
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
    }
}
