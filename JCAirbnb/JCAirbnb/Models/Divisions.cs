using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class Divisions
    {
        [Display(Name = "Guest(s)")]
        public int Guest { get; set; }

        [Display(Name = "Bedroom(s)")]
        public int Bedroom { get; set; }

        [Display(Name = "Bed(s)")]
        public int Bed { get; set; }

        [Display(Name = "Bath(s)")]
        public int Bath { get; set; }

        [Display(Name = "Private Bath(s)")]
        public int PrivateBath { get; set; }
    }
}
