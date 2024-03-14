using JCAirbnb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Client.Models
{
    public class CheckOutViewModel
    {
        public Reservation Reservation { get; set; }

        public Ratings Ratings { get; set; }

        public string Comment { get; set; }
    }
}
