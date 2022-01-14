using System;
using JCAirbnb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Employee.Models
{
    public class ManageReservationsEditViewModel
    {
        public Reservation Reservation { get; set; }

        public List<CheckList> CheckLists { get; set; }

        public string CheckListId { get; set; }
    }
}
