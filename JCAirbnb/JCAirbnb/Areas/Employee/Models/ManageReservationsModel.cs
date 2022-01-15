using System;
using JCAirbnb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Areas.Employee.Models
{
    public class ManageReservationsModel
    {
        [Display(Name = "Reservation")]
        public Reservation Reservation { get; set; }

        [Display(Name = "Check list items")]
        public List<CheckListItem> CheckListItems { get; set; }

    }
}
