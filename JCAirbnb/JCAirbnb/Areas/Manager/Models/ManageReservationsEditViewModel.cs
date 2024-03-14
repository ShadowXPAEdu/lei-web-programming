using JCAirbnb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Manager.Models
{
    public class ManageReservationsEditViewModel
    {
        public Reservation Reservation { get; set; }

        public List<CheckList> CheckLists { get; set; }

        public string ReservationCheckListId { get; set; }

        public string DeliveryCheckListId { get; set; }
    }
}
